using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using DayZTediratorToolz.Helpers;
using DayZTediratorToolz.Helpers.CustomControls;
using DayZTediratorToolz.Models;
using DayZTediratorToolz.Services;
using DayZTediratorToolz.Services.ToolConfigService;
using MaterialDesignThemes.Wpf;
using MoreLinq;
using MoreLinq.Extensions;
using Newtonsoft.Json;
using PropertyChanged;
using Syncfusion.Data;
using Syncfusion.Data.Extensions;
using Syncfusion.UI.Xaml.Grid;

namespace DayZTediratorToolz.Views
{
    [AddINotifyPropertyChangedInterface]
    public partial class TypesEditorView : BaseView
    {
        private readonly ITypesConvertorService _typesConvertorService;
        private readonly IGeneralHelperService _generalHelperService;
        private readonly INotificationService _notificationService;
        private readonly IToolConfigService _toolConfigService;
        private DayZTediratorConstants.TypesViewTools _activeTool;
        private string _subEditorTitle;
        private string _typesPath;
        private ObservableCollection<ObservableCollection<TypeCollectionModel.Type>> _typeCollection;
        private object _subCollection;
        private ObservableCollection<TypeCollectionModel.Category> _catList;
        private TypeCollectionModel.Type _currentTypeObj;
        private bool _isProcessingTypes;
        private bool _isExportingTypes;
        private string _searchText;

        private const string _titleKey = "TypesEditorTitle";

        public TypesCfg TypesConfig { get; set; }

        public override ViewMenuData ViewMenuData { get; set; }

        public string SubEditorTitle
        {
            get => _subEditorTitle;
            set => _subEditorTitle = value;
        }

        public string TypesPath
        {
            get => _typesPath;
            set => _typesPath = value;
        }

        public ObservableCollection<ObservableCollection<TypeCollectionModel.Type>> TypeCollection
        {
            get => _typeCollection;
            set => _typeCollection = value;
        }

        public ObservableCollection<TypeCollectionModel.Type> CurrentTypeSlice
        {
            get;
            set;
        }

        private int typeSliceIndex { get; set; }

        public object SubCollection
        {
            get => _subCollection;
            set => _subCollection = value;
        }

        public ObservableCollection<TypeCollectionModel.Category> CatList
        {
            get => _catList;
            set => _catList = value;
        }

        public bool IsDataPaged
        {
            get
            {
                if (TypeCollection == null)
                    return false;

                return TypeCollection.Count() > 1;
            }
        }

        public int IsDataPagedGridRowSpan => IsDataPaged ? 1 : 2;
        public bool IsDataLoaded => TypeCollection != null;

        public bool TypeIsSelected {get => CurrentTypeObj != null;}
        public bool TypeEditorSelected { get => ActiveTool == DayZTediratorConstants.TypesViewTools.Editor;}
        public bool TypeBatchToolsSelected { get => ActiveTool == DayZTediratorConstants.TypesViewTools.Batch;}
        public bool TypeImporterSelected { get => ActiveTool == DayZTediratorConstants.TypesViewTools.Import;}

        public TypeCollectionModel.Type CurrentTypeObj
        {
            get => _currentTypeObj;
            set => _currentTypeObj = value;
        }

        public bool IsProcessingTypes
        {
            get => _isProcessingTypes;
            set => _isProcessingTypes = value;
        }

        public bool IsExportingTypes
        {
            get => _isExportingTypes;
            set => _isExportingTypes = value;
        }

        public DayZTediratorConstants.TypesViewTools ActiveTool
        {
            get => _activeTool;
            set
            {
                _activeTool = value == null ? _activeTool: value;
            }
        }


        public ICommand DeleteItem => new RelayCommand(o => {
            if(SubCollection is ObservableCollection<TypeCollectionModel.Usage>)
                (SubCollection as ObservableCollection<TypeCollectionModel.Usage>).Remove(o as TypeCollectionModel.Usage);
            else
                (SubCollection as ObservableCollection<TypeCollectionModel.Value>).Remove(o as TypeCollectionModel.Value);
        }, o => true);

        public ICommand SelectSlice => new RelayCommand(o =>
        {
            ClearGroups();
            CurrentTypeSlice = null;
            CurrentTypeSlice = (o as ObservableCollection<TypeCollectionModel.Type>);
            GenGroups(false);
        }, o => true);

        public ICommand SetCategory => new RelayCommand(o =>
        {
            var typeID = CurrentTypeObj.UID;
            CurrentTypeObj.Category.Name = (o as TypeCollectionModel.Category).Name;
            GenGroups();
            SearchAndSelect(typeID);
        }, o => true);

        private void SearchAndSelect(string searchContent)
        {
            TypesGrid.SearchHelper.CanHighlightSearchText = false;
            TypesGrid.SearchHelper.AllowFiltering = false;

            TypesGrid.SearchHelper.FindNext(searchContent);
            var list = TypesGrid.SearchHelper.GetSearchRecords();
            var recordItem = (list[0].Record as RecordEntry).Data;
            var resolveToRowIndex = TypesGrid.ResolveToRowIndex(recordItem);
            int recordIndex = TypesGrid.ResolveToRecordIndex(resolveToRowIndex);
            TypesGrid.SelectedIndex = recordIndex;
            TypesGrid.SearchHelper.Search("");
            TypesGrid.SearchHelper.ClearSearch();

            TypesGrid.SearchHelper.CanHighlightSearchText = true;
            TypesGrid.SearchHelper.AllowFiltering = true;
        }


        // Constructor
        public TypesEditorView(ITypesConvertorService typesConvertorService,
            IGeneralHelperService generalHelperService,
            INotificationService notificationService,
            IToolConfigService toolConfigService)
        {
            ViewMenuData = new(){ViewIndex = 2, ViewLabel = _titleKey.LoadFromRes<string>(), ViewIcon = PackIconKind.FoodApple, ViewType = DayZTediratorConstants.ViewTypes.VanillaTool};

            _typesConvertorService = typesConvertorService;
            _generalHelperService = generalHelperService;
            _notificationService = notificationService;
            _toolConfigService = toolConfigService;
            ActiveTool = DayZTediratorConstants.TypesViewTools.Editor;

            TypesConfig = _toolConfigService.GetConfigObj(DayZTediratorConstants.Tools.Types) as TypesCfg;
            InitializeComponent();
            DataContext = this;
            FileInputControl.ConfigureHelperService(_generalHelperService);

            TypesGrid.GroupColumnDescriptions = new GroupColumnDescriptions();

        }

        private async void OpenFileDialogButtonClicked(object sender, RoutedEventArgs e)
        {
            if (IsProcessingTypes)
                return;

            var localTypesPath = _generalHelperService.GetPathFromUser(DayZTediratorConstants.PathTypes.TypesXml,
                DayZTediratorConstants.DialogTypes.Open);

            await OpenTypesFile(localTypesPath);


            IsProcessingTypes = false;
        }

        private async Task OpenTypesFile(string localTypesPath)
        {
            if (System.IO.File.Exists(localTypesPath))
            {
                IsProcessingTypes = true;

                if (TypeCollection != null)
                {
                    TypesGrid.ClearFilters();
                    TypesGrid.ClearSelections(false);
                    ClearGroups();
                    CurrentTypeSlice = null;
                    TypeCollection = null;
                    _typesConvertorService.ResetTypesCollection();
                }

                _typesConvertorService
                    .SetInitData(System.IO.File.ReadAllText(localTypesPath));
                await _typesConvertorService
                    .InitializeTypes();

                if (_typesConvertorService.TypesLoadedSuccesfully())
                {
                    ClearGroups();
                    TypesPath = localTypesPath;
                    var typeCollectionBatched = MoreEnumerable
                        .Batch(_typesConvertorService.GetTypes().Type, 1000, t => t.ToObservableCollection())
                        .ToObservableCollection();
                    TypeCollection = typeCollectionBatched;
                    GenGroups();
                }
                else
                {
                    _notificationService.SetNotificationContent($"Types Not Loaded",
                            $"There are errors in the file at: {localTypesPath}")
                        .NotifyFailure();

                    TypesPath = string.Empty;
                }

                if (TypeCollection != null)
                {
                    TypesConfig.RecentTypesHistory.UpdateHistory(localTypesPath);
                    _notificationService.SetNotificationContent($"Types Loaded",
                            $"Successfully loaded types file from: {localTypesPath}")
                        .NotifySuccess();

                    _notificationService.SetNotificationContent($"Types Loaded",
                            $"Loaded {TypeCollection.DeepCount()} types.")
                        .NotifyInfo();

                    List<TypeCollectionModel.Category> filteredCats = new List<TypeCollectionModel.Category>();
                    ForEachExtension.ForEach(TypeCollection, t => filteredCats.AddRange(t
                        .Where(t => !string.IsNullOrEmpty(t.Category.Name))
                        .Select(t => t.Category)
                        .GroupBy(c => c.Name)
                        .Select(grp => grp.FirstOrDefault()).ToList()));

                    CatList = new ObservableCollection<TypeCollectionModel.Category>(DistinctByExtension
                        .DistinctBy(filteredCats, t => t.Name).Select(c => new TypeCollectionModel.Category {Name = c.Name}));

                    CurrentTypeSlice = _typeCollection[0];
                }
            }
        }

        private void ClearGroups()
        {
            TypesGrid.GroupColumnDescriptions.Clear();
        }

        private void GenGroups(bool expand = true)
        {
            ClearGroups();
            TypesGrid.GroupColumnDescriptions.Add(new GroupColumnDescription()
            {
                ColumnName = "CategoryRef"
            });
            if(expand)
                TypesGrid.ExpandAllGroup();
        }

        private void TypesGrid_OnAutoGeneratingColumn(object sender, AutoGeneratingColumnArgs e)
        {
            if (e.Column.MappingName == "Category.Name")
                e.Column.HeaderText = "Category";

            if (e.Column.MappingName == "CategoryRef")
                e.Column.HeaderText = "Category";

            if (e.Column.MappingName.Contains("Flags."))
                e.Column.HeaderText = e.Column.HeaderText.Replace("Flags.", "");

            if (e.Column.MappingName.In("UsagesCountMsg", "TiersCountMsg"))
                e.Cancel = true;

            if (e.Column.MappingName == "UID")
                e.Column.MaximumWidth = 1;
        }

        private void TypesGrid_OnSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count == 0 | (e.AddedItems.Count == 1 && TypesGrid.SelectedItems.Count > 1))
                CurrentTypeObj = null;
            else
            {
                var rowData = (e.AddedItems[0] as GridRowInfo).RowData;
                if (rowData is TypeCollectionModel.Type)
                    CurrentTypeObj = rowData as TypeCollectionModel.Type;
                else
                    CurrentTypeObj = null;
            }

        }

        private void DeleteTypeClicked(object sender, RoutedEventArgs e)
        {
            if (TypesGrid.SelectedItems.Count > 1)
                RemoveTypesByList(TypesGrid.SelectedItems.ToList());
            else
                CurrentTypeSlice.Remove(CurrentTypeObj);
        }

        private void RemoveTypesByList(List<object> types)
        {
            foreach (TypeCollectionModel.Type type in types)
            {
                CurrentTypeSlice.Remove(type);
            }
        }

        private void AddTypeClicked(object sender, RoutedEventArgs e)
        {

        }

        private void ApplySubTypeChangesClicked(object sender, RoutedEventArgs e)
        {

        }

        private void CancelSubTypeChangesClicked(object sender, RoutedEventArgs e) => DHost.IsOpen = false;

        private void EditUsages(object sender, RoutedEventArgs e)
        {
            SubEditorTitle = "Usages";
            SubCollection = CurrentTypeObj.Usages;
            DHost.IsOpen = true;
        }

        private void EditTiers(object sender, RoutedEventArgs e)
        {
            SubEditorTitle = "Tiers";
            SubCollection = CurrentTypeObj.Tiers;
            DHost.IsOpen = true;
        }

        private async Task SaveFile()
        {
            if (IsExportingTypes || TypeCollection == null)
                return;


            TypesPath = _generalHelperService.GetPathFromUser(DayZTediratorConstants.PathTypes.TypesXml,
                DayZTediratorConstants.DialogTypes.Save);

            if (System.IO.File.Exists(TypesPath))
            {
                IsExportingTypes = true;
                var jsonState = await _typesConvertorService
                    .GetSerializedTypesXml();
                var xmlDoc = JsonConvert.DeserializeXmlNode(jsonState);

                XmlDeclaration xmldecl;
                xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlElement root = xmlDoc.DocumentElement;
                xmlDoc.InsertBefore(xmldecl, root);

                var formattedXML = xmlDoc.OuterXml.FormatXml();

                System.IO.File.WriteAllText(TypesPath, formattedXML);

                _notificationService.SetNotificationContent($"Types Exported",
                            $"Successfully exported types file to: {TypesPath}")
                        .NotifySuccess();
                _notificationService.SetNotificationContent($"Types Exported",
                            $"Exported a total of {TypeCollection.DeepCount()} types.")
                        .NotifyInfo();
            }


            IsExportingTypes = false;
        }

        private void AddActiveEditorArrayItemClicked(object sender, RoutedEventArgs e)
        {
            if (SubCollection == null)
                return;

            if(SubCollection is ObservableCollection<TypeCollectionModel.Usage>)
                (SubCollection as ObservableCollection<TypeCollectionModel.Usage>).Add(new TypeCollectionModel.Usage());
            else
                (SubCollection as ObservableCollection<TypeCollectionModel.Value>).Add(new TypeCollectionModel.Value());
        }

        private void TypeEditorButtonClicked(object sender, RoutedEventArgs e) => ActiveTool = DayZTediratorConstants.TypesViewTools.Editor;

        private void TypeToolsButtonClicked(object sender, RoutedEventArgs e) => ActiveTool = DayZTediratorConstants.TypesViewTools.Batch;

        private void TypeImportClicked(object sender, RoutedEventArgs e) => ActiveTool = DayZTediratorConstants.TypesViewTools.Import;

        private void CatButton_Click(object sender, RoutedEventArgs e)
        {
            var addButton = sender as FrameworkElement;
            if (addButton != null)
            {
                addButton.ContextMenu.IsOpen = true;
            }
        }

        public override void CloseView()
        {
            _toolConfigService.SaveConfigData();
        }

        private async void FileInputControl_OnFileOpened(object sender, FileEventArgs args)
        {
            await OpenTypesFile(args.FilePath);
        }

        private async void FileInputControl_OnFileSaved(object sender, FileEventArgs args)
        {
            await SaveFile();
        }

        private void FileInputControl_OnFileExported(object sender, FileEventArgs args)
        {

        }
    }
}