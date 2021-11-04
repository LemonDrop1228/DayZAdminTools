namespace DayZTediratorToolz.Helpers
{
    public static class DayZTediratorConstants
    {
        public enum AdvBttnContentType
        {
            Both,
            Image,
            Txt
        }

        public enum Views
        {
            Home,
            Admin,
            TypesEditor,
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
            Xml,
            Json
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