
using BookStoreTM.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreTM.Models.Entities
{
    public class AppDbContext : DbContext
    {
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<News> News { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Adv> Advs { get; set; }
        //public DbSet<CommonAbstract> CommonAbstracts { get; set; }
        public DbSet<OrderBook> OrderBooks { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptDetails> ReceiptDetails { get; set; }
    }
}
