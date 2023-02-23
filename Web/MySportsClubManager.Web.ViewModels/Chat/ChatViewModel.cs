namespace MySportsClubManager.Web.ViewModels.Chat
{
    using System.Collections.Generic;

    using MySportsClubManager.Web.ViewModels.Message;

    public class ChatViewModel
    {
        public string Receiver { get; set; } = null;

        public string ReceiverImage { get; set; } = null;

        public string YourImage { get; set; } = null;

        public IList<MessageInListViewModel> Messages { get; set; } = null!;
    }
}
