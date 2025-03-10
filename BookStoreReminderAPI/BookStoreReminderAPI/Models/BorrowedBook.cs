namespace BookStoreReminderAPI.Models
{
    public class BorrowedBook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int BookId { get; set; }
        public string BorrowerEmail { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public Book Book { get; set; }
    }
}
