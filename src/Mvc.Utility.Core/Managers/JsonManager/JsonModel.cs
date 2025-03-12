namespace Mvc.Utility.Core.Managers.JsonManager
{
    public class JsonModel
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public bool? Success { set; get; }
        public string[] Messages { set; get; }
        public string RedirectToUrl { get; set; }
        public int RedirectToNextState { get; set; }
    }
}