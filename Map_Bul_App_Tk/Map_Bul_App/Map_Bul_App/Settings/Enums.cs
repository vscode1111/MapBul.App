namespace Map_Bul_App.Settings
{
    public enum IdType
    {
        LocalId,
        ServerId
    }

    public enum ButtonAction
    {
        Ok,
        Cancel
    }

    public enum ArticleType
    {
        Article,
        Event
    }

    public  enum Pages
    {
        Map,
        Calendar,
        Articles,
        Favorites
    }
    
    public static class UserTypesMobile
    {
        public const string Guide = "guide";
        public const string Tenant = "tenant";
        public const string Guest = "guest";

        public static string GetMobileTypeByServerType(string serverType)
        {
            switch (serverType)
            {
                case "edit":
                case "journ":
                case "guide":
                case "admin":
                    return Guide;
                case "tenant":
                    return Tenant;
                case "guest":
                    return Guest;
            }
            return Guide;
        }
    }

    public enum CurrentPinsOnMap
    {
        Filter,
        FindByText,
        Favorites
    }
   
}
