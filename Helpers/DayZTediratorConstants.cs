namespace DayZTediratorToolz.Helpers
{
    public static class DayZTediratorConstants
    {
        public enum Views
        {
            Home,
            Admin,
            TypesEditor,
            EffectAreaEditor
            MapTool
        }

        public enum ViewTypes
        {
            Info,
            Admin,
            VanillaTool,
            ModTool,
            Dialog
        }

        public enum Tools
        {
            Admin,
            Types
        }

        public enum PathTypes
        {
            TypesXml
        }

        public enum DialogTypes
        {
            Open,
            Save,
            Export
        }

        public enum TypesViewTools
        {
            Editor,
            Batch,
            Import
        }

        public enum ServiceStates
        {
            Unknown,
            Standby,
            Busy,
            Failed
        }
    }
}
