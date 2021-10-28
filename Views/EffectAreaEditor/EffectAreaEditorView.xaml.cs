using DayZTediratorToolz.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using static DayZTediratorToolz.Models.ToxicEffectAreaModel;

namespace DayZTediratorToolz.Views.EffectAreaEditor
{
    public partial class EffectAreaEditorView : BaseView
    {
        public ToxicEffectConfig ToxicEffectAreaObj { get; set; }
        public EffectAreaEditorView()
        {
            SetupTestData();
            InitializeComponent();
        }

        private void SetupTestData()
        {
            ToxicEffectAreaObj = new ToxicEffectConfig()
            {
                Areas = new List<Area>()
                {
                    new Area()
                    {
                        AreaName = "Lori",
                        Type = "Ipsum",
                        TriggerType = "Scrotum",
                        Data = new Data
                        {
                            Pos = new List<int>(){1,1,1},
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
                        }
                    },
                    new Area()
                    {
                        AreaName = "Dory",
                        Type = "Sipsum",
                        TriggerType = "Dumdum",
                        Data = new Data
                        {
                            Pos = new List<int>(){1,1,1},
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
                        }
                    }

                },
                SafePositions = new List<int[]>()
                {
                    new int[] {1,2},
                    new int[] {1,2}
                }


            };
        }
    }
}
