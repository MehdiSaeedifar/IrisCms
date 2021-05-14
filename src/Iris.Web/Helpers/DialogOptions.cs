namespace Iris.Web.Helpers
{
    public class DialogOptions
    {
        public DialogOptions()
        {
            Width = "300";
            Height = "400";
            ShowEffect = "clip";
            ShowDir = "left";
            HideEffect = "clip";
            HideDir = "right";
        }

        public string Width { get; set; }
        public string Height { get; set; }
        public string ShowEffect { get; set; }
        public string ShowDir { get; set; }
        public string HideEffect { get; set; }
        public string HideDir { get; set; }
        public string IsModal { get; set; }
        public string Title { get; set; }
    }
}