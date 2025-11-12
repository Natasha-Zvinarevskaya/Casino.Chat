namespace Casino.Chat.DataContext
{
    public class Letter
    {
        /// <summary>
        /// Ид письма
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Ид отправителя
        /// </summary>
        public int IdSender { get; set; }
        /// <summary>
        /// Ид получателя
        /// </summary>
        public int IdRecipient { get; set; }
        /// <summary>
        /// Ид игры
        /// </summary>
        public int? GameId { get; set; }
        /// <summary>
        /// Время отправления
        /// </summary>
        public DateTime TimeSending { get; set; }
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

    }
}
