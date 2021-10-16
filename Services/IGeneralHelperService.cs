using System;
using System.Diagnostics;
using Ookii.Dialogs.Wpf;
using static DayZTediratorToolz.Helpers.DayZTediratorConstants;

namespace DayZTediratorToolz.Services
{
    public interface IGeneralHelperService
    {
        string GetPathFromUser(PathTypes pathType, DialogTypes dialogTypes);
    }

    public class GeneralHelperService : IGeneralHelperService
    {
        public string GetPathFromUser(PathTypes pathType, DialogTypes dialogTypes)
        {
            string result = pathType switch
            {
                PathTypes.TypesXml => GetPath("XML Files | *.xml", "{0} Types Config File", dialogTypes),
                _ => string.Empty
            };

            return result;
        }

        private string GetPath(string _filter, string _title, DialogTypes dialogTypes)
        {
            string res = null;

            switch (dialogTypes)
            {
                case DialogTypes.Open:
                    var ofd = new VistaOpenFileDialog()
                    {
                        Filter = _filter,
                        Title = string.Format(_title, "Load")
                    };

                    if (ofd.ShowDialog() == true)
                        res = ofd.FileName;
                    break;
                case DialogTypes.Save:
                    var sfd = new VistaSaveFileDialog()
                    {
                        Filter = _filter,
                        Title = string.Format(_title, "Save")
                    };

                    if (sfd.ShowDialog() == true)
                        res = sfd.FileName;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dialogTypes), dialogTypes, null);
            }
            
            
            return res;
        }

    }
}