using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class Transaction
    {
        [Key]

        public int TransactionId {  get; set; }
        [Range(1,int.MaxValue,ErrorMessage ="please select a category")]
        public int CategoryId {  get; set; }
        public Category? Category {  get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Amount should be greater than zero")]
        public int Amount {  get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string ?Note { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
        [NotMapped]
        public string? categoryTitleWithIcon
        {
            get
            {
                return Category == null ? "" : Category.Icon + "" + Category.Title;
            }
        }
        [NotMapped]
        public string? FormattedAmount
        {
            get
            {
                return ((Category == null||Category.Type=="Expense") ? "_" : "+")+Amount.ToString("C0");
            }
        }
    }
}
