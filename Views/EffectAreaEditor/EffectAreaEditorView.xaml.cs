using DayZTediratorToolz.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LoremNET;
using PropertyChanged;
using static DayZTediratorToolz.Models.ToxicEffectAreaModel;

namespace DayZTediratorToolz.Views.EffectAreaEditor
{
    [AddINotifyPropertyChangedInterface]
    public partial class EffectAreaEditorView : BaseView
    {
        public ToxicEffectConfig ToxicEffectAreaObj { get; set; }

        public ObservableCollection<Area> AreasCollection
        {
            get => ToxicEffectAreaObj.Areas;
        }

        public EffectAreaEditorView()
        {
            SetupTestData();
            InitializeComponent();
            this.DataContext = this;
        }

        private void SetupTestData()
        {
            ToxicEffectAreaObj = new ToxicEffectConfig()
            {
                Areas = new ObservableCollection<Area>(GenerateAreasForTesting()),
                SafePositions = new ObservableCollection<int[]>()
                {
                    new int[] {1,2},
                    new int[] {1,2}
                }
            };
        }

        private List<Area> GenerateAreasForTesting()
        {
            List<Area> res = new();
            for (int i = 0; i < 99; i++)
            {
                res.Add(
                    new Area
                    {
                        AreaName = GetRandoString(),
                        Type = GetRandoString(),
                        TriggerType = GetRandoString(),
                        Data = new Data
                        {
                            Pos = new ObservableCollection<int>(new int[] {1,2,3}),
                            Radius = 0,
                            PosHeight = 0,
                            NegHeight = 0,
                            InnerRingCount = 0,
                            InnerPartDist = 0,
                            OuterRingToggle = 0,
                            OuterPartDist = 0,
                            OuterOffset = 0,
                            VerticalLayers = 0,
                            VerticalOffset = 0,
                            ParticleName = ""
                        },
                        PlayerData = new PlayerData
                        {
                            AroundPartName = GetRandoString(),
                            TinyPartName = GetRandoString(),
                            PPERequesterType = GetRandoString()
                        }
                    }

                );
            }

            return res;
        }

        private static string GetRandoString()
        {
            return Lorem.Words(2,7).Replace(" ", String.Empty);
        }
    }
}
