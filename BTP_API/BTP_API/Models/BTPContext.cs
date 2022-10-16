using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BTP_API.Models
{
    public partial class BTPContext : DbContext
    {
        public BTPContext()
        {
        }

        public BTPContext(DbContextOptions<BTPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Exchange> Exchanges { get; set; }
        public virtual DbSet<ExchangeBill> ExchangeBills { get; set; }
        public virtual DbSet<ExchangeDetail> ExchangeDetails { get; set; }
        public virtual DbSet<ExchangeRequest> ExchangeRequests { get; set; }
        public virtual DbSet<FavoriteBookList> FavoriteBookLists { get; set; }
        public virtual DbSet<FavoritePostList> FavoritePostLists { get; set; }
        public virtual DbSet<FavoriteUserList> FavoriteUserLists { get; set; }
        public virtual DbSet<Fee> Fees { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Rent> Rents { get; set; }
        public virtual DbSet<RentBill> RentBills { get; set; }
        public virtual DbSet<RentDetail> RentDetails { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<ShipInfo> ShipInfos { get; set; }
        public virtual DbSet<User> Users { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseNpgsql("Host=localhost;Database=BTP;Username=postgres;Password=canhnamakashi");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã sách");

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Tác giả");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .HasComment("Mã loại");

                entity.Property(e => e.CoverPrice).HasComment("Giá bìa");

                entity.Property(e => e.DepositPrice).HasComment("Giá cọc");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasComment("Mô tả nội dung");

                entity.Property(e => e.Image).IsRequired();

                entity.Property(e => e.IsExchange).HasComment("Trao đổi?");

                entity.Property(e => e.IsReady).HasComment("Sẵn sàng?");

                entity.Property(e => e.IsRent).HasComment("Thuê?");

                entity.Property(e => e.IsTrade).HasComment("Đang giao dịch?");

                entity.Property(e => e.Language)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Ngôn ngữ");

                entity.Property(e => e.NumberOfDays).HasComment("Số ngày");

                entity.Property(e => e.NumberOfPages).HasComment("Số trang");

                entity.Property(e => e.PostedDate).HasComment("Ngày đăng");

                entity.Property(e => e.Publisher)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("Nhà xuất bản");

                entity.Property(e => e.RentFee).HasComment("Phí thuê?");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Trạng thái");

                entity.Property(e => e.StatusBook)
                    .IsRequired()
                    .HasComment("Trạng thái sách");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("Tên sách");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("Mã người dùng");

                entity.Property(e => e.Weight).HasComment("Trọng lượng");

                entity.Property(e => e.Year).HasComment("Năm xuất bản");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Book_Category");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Book_User");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã loại");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Tên loại");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã bình luận");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasComment("Nội dung");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("timestamp without time zone")
                    .HasComment("Ngày bình luận");

                entity.Property(e => e.PostId)
                    .HasColumnName("PostID")
                    .HasComment("Mã bài đăng");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("Mã người dùng");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_User");
            });

            modelBuilder.Entity<Exchange>(entity =>
            {
                entity.ToTable("Exchange");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã giao dịch đổi");

                entity.Property(e => e.Date).HasComment("Ngày giao dịch");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Trạng thái");

                entity.Property(e => e.UserId1)
                    .HasColumnName("UserID1")
                    .HasComment("Mã người dùng 1");

                entity.Property(e => e.UserId2)
                    .HasColumnName("UserID2")
                    .HasComment("Mã người dùng 2");

                entity.HasOne(d => d.UserId1Navigation)
                    .WithMany(p => p.ExchangeUserId1Navigations)
                    .HasForeignKey(d => d.UserId1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Exchange_User1");

                entity.HasOne(d => d.UserId2Navigation)
                    .WithMany(p => p.ExchangeUserId2Navigations)
                    .HasForeignKey(d => d.UserId2)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Exchange_User2");
            });

            modelBuilder.Entity<ExchangeBill>(entity =>
            {
                entity.ToTable("ExchangeBill");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã hóa đơn giao dịch đổi");

                entity.Property(e => e.DepositFee).HasComment("Phí đặt cọc");

                entity.Property(e => e.ExchangeId)
                    .HasColumnName("ExchangeID")
                    .HasComment("Mã giao dịch đổi");

                entity.Property(e => e.FeeId1)
                    .HasColumnName("FeeID1")
                    .HasComment("Mã phí 1");

                entity.Property(e => e.FeeId2)
                    .HasColumnName("FeeID2")
                    .HasComment("Mã phí 2");

                entity.Property(e => e.FeeId3)
                    .HasColumnName("FeeID3")
                    .HasComment("Mã phí 3");

                entity.Property(e => e.Flag).HasComment("Cờ");

                entity.Property(e => e.IsPaid).HasComment("Đã thanh toán?");

                entity.Property(e => e.PaidDate)
                    .HasColumnType("timestamp without time zone")
                    .HasComment("Ngày thanh toán");

                entity.Property(e => e.Payments)
                    .HasMaxLength(50)
                    .HasComment("Phương thức thanh toán");

                entity.Property(e => e.RecallDate).HasComment("Ngày thu hồi");

                entity.Property(e => e.ReceiveDate).HasComment("Ngày nhận");

                entity.Property(e => e.RefundDate).HasComment("Ngày hoàn trả");

                entity.Property(e => e.SendDate).HasComment("Ngày gửi");

                entity.Property(e => e.TotalAmount).HasComment("Tổng tiền");

                entity.Property(e => e.TotalBook).HasComment("Tổng sách");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("Mã người dùng");

                entity.HasOne(d => d.Exchange)
                    .WithMany(p => p.ExchangeBills)
                    .HasForeignKey(d => d.ExchangeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchangeBill_Exchange");

                entity.HasOne(d => d.FeeId1Navigation)
                    .WithMany(p => p.ExchangeBillFeeId1Navigations)
                    .HasForeignKey(d => d.FeeId1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchangeBill_Fee1");

                entity.HasOne(d => d.FeeId2Navigation)
                    .WithMany(p => p.ExchangeBillFeeId2Navigations)
                    .HasForeignKey(d => d.FeeId2)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchangeBill_Fee2");

                entity.HasOne(d => d.FeeId3Navigation)
                    .WithMany(p => p.ExchangeBillFeeId3Navigations)
                    .HasForeignKey(d => d.FeeId3)
                    .HasConstraintName("FK_ExchangeBill_Fee3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ExchangeBills)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchangeBill_User");
            });

            modelBuilder.Entity<ExchangeDetail>(entity =>
            {
                entity.ToTable("ExchangeDetail");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã chi tiết giao dịch đổi");

                entity.Property(e => e.AfterStatusBook1).HasComment("Trạng thái sách 1 sau giao dịch");

                entity.Property(e => e.AfterStatusBook2).HasComment("Trạng thái sách 2 sau giao dịch");

                entity.Property(e => e.BeforeStatusBook1).HasComment("Trạng thái sách 1 trước giao dịch");

                entity.Property(e => e.BeforeStatusBook2).HasComment("Trạng thái sách 2 trước giao dịch");

                entity.Property(e => e.Book1Id)
                    .HasColumnName("Book1ID")
                    .HasComment("Mã sách 1");

                entity.Property(e => e.Book2Id)
                    .HasColumnName("Book2ID")
                    .HasComment("Mã sách 2");

                entity.Property(e => e.ExchangeId)
                    .HasColumnName("ExchangeID")
                    .HasComment("Mã giao dịch đổi");

                entity.Property(e => e.ExpiredDate).HasComment("Ngày hết hạn");

                entity.Property(e => e.Flag).HasComment("Cờ");

                entity.Property(e => e.RequestTime)
                    .HasColumnType("timestamp without time zone")
                    .HasComment("Thời gian tạo");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Trạng thái chi tiết giao dịch");

                entity.Property(e => e.StorageStatusBook1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Trạng thái lưu trữ sách1");

                entity.Property(e => e.StorageStatusBook2)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Trạng thái lưu trữ sách 2");

                entity.HasOne(d => d.Book1)
                    .WithMany(p => p.ExchangeDetailBook1s)
                    .HasForeignKey(d => d.Book1Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchanegDetail_Book1");

                entity.HasOne(d => d.Book2)
                    .WithMany(p => p.ExchangeDetailBook2s)
                    .HasForeignKey(d => d.Book2Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchanegDetail_Book2");

                entity.HasOne(d => d.Exchange)
                    .WithMany(p => p.ExchangeDetails)
                    .HasForeignKey(d => d.ExchangeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchanegDetail_Exchange");
            });

            modelBuilder.Entity<ExchangeRequest>(entity =>
            {
                entity.ToTable("ExchangeRequest");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã yêu cầu đổi");

                entity.Property(e => e.BookId)
                    .HasColumnName("BookID")
                    .HasComment("Mã sách");

                entity.Property(e => e.BookOfferId)
                    .HasColumnName("BookOfferID")
                    .HasComment("Mã sách yêu cầu");

                entity.Property(e => e.Flag).HasComment("Cờ");

                entity.Property(e => e.IsAccept).HasComment("Đồng ý?");

                entity.Property(e => e.IsNewest).HasComment("Mới nhất?");

                entity.Property(e => e.RequestTime)
                    .HasColumnType("timestamp without time zone")
                    .HasComment("Thời gian yêu cầu");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Trạng thái yêu cầu");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.ExchangeRequestBooks)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchangeRequest_Book");

                entity.HasOne(d => d.BookOffer)
                    .WithMany(p => p.ExchangeRequestBookOffers)
                    .HasForeignKey(d => d.BookOfferId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExchangeRequest_BookOffer");
            });

            modelBuilder.Entity<FavoriteBookList>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã sách yêu thích");

                entity.Property(e => e.BookId)
                    .HasColumnName("BookID")
                    .HasComment("Mã sách");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("Mã người dùng");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.FavoriteBookLists)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavariteBookLists_Book");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavoriteBookLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavariteBookLists_User");
            });

            modelBuilder.Entity<FavoritePostList>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã bài đăng yêu thích");

                entity.Property(e => e.PostId)
                    .HasColumnName("PostID")
                    .HasComment("Mã bài đăng");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("Mã người dùng");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.FavoritePostLists)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavoritePostLists_Post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavoritePostLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavoritePostLists_User");
            });

            modelBuilder.Entity<FavoriteUserList>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã yêu thích bài đăng");

                entity.Property(e => e.FavoriteUserId)
                    .HasColumnName("FavoriteUserID")
                    .HasComment("Mã người dùng được yêu thích");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("Mã người dùng yêu thích");

                entity.HasOne(d => d.FavoriteUser)
                    .WithMany(p => p.FavoriteUserListFavoriteUsers)
                    .HasForeignKey(d => d.FavoriteUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavoriteUserLists_FavoriteUser");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FavoriteUserListUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FavoriteUserLists_User");
            });

            modelBuilder.Entity<Fee>(entity =>
            {
                entity.ToTable("Fee");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã phí");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasComment("Mã code phí");

                entity.Property(e => e.IsActive).HasComment("Đang hoạt động");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Tên phí");

                entity.Property(e => e.Price).HasComment("Giá");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã đánh giá");

                entity.Property(e => e.BookId)
                    .HasColumnName("BookID")
                    .HasComment("Mã sách");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasComment("Nội dung");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("timestamp without time zone")
                    .HasComment("Ngày đánh giá");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("Mã người dùng");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Feedback_Book");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Feedback_User");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã bài đăng");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasComment("Nội dung");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("timestamp without time zone")
                    .HasComment("Ngày đăng");

                entity.Property(e => e.Hashtag)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Thẻ");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasComment("Ảnh");

                entity.Property(e => e.IsHide).HasComment("Đã ẩn?");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Trạng thái bài đăng");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("Mã người dùng");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Post_User");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshToken");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã refresh token");

                entity.Property(e => e.ExpiredDate)
                    .HasColumnType("timestamp without time zone")
                    .HasComment("Ngày hết hạn");

                entity.Property(e => e.IsRevoked).HasComment("Đã hủy");

                entity.Property(e => e.IsUsed).HasComment("Đã sử dụng?");

                entity.Property(e => e.IssueDate)
                    .HasColumnType("timestamp without time zone")
                    .HasComment("Ngày đăng ký");

                entity.Property(e => e.JwtId)
                    .IsRequired()
                    .HasColumnName("JwtID")
                    .HasComment("Mã access token");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasComment("Chuỗi token");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("Mã người dùng");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RefreshToken_User");
            });

            modelBuilder.Entity<Rent>(entity =>
            {
                entity.ToTable("Rent");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã giao dịch thuê");

                entity.Property(e => e.Date).HasComment("Ngày giao dịch");

                entity.Property(e => e.OwnerId)
                    .HasColumnName("OwnerID")
                    .HasComment("Mã người cho thuê");

                entity.Property(e => e.RenterId)
                    .HasColumnName("RenterID")
                    .HasComment("Mã người thuê");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Trạng thái giao dịch");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.RentOwners)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rent_Owner");

                entity.HasOne(d => d.Renter)
                    .WithMany(p => p.RentRenters)
                    .HasForeignKey(d => d.RenterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rent_Renter");
            });

            modelBuilder.Entity<RentBill>(entity =>
            {
                entity.ToTable("RentBill");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã hóa đơn giao dịch thuê");

                entity.Property(e => e.DepositFee).HasComment("Phí đặt cocj");

                entity.Property(e => e.FeeId1)
                    .HasColumnName("FeeID1")
                    .HasComment("Mã phí 1");

                entity.Property(e => e.FeeId2)
                    .HasColumnName("FeeID2")
                    .HasComment("Mã phí 2");

                entity.Property(e => e.FeeId3)
                    .HasColumnName("FeeID3")
                    .HasComment("Mã phí 3");

                entity.Property(e => e.Flag).HasComment("Cờ");

                entity.Property(e => e.IsPaid).HasComment("Đã thanh toán?");

                entity.Property(e => e.PaidDate)
                    .HasColumnType("timestamp without time zone")
                    .HasComment("Ngày thanh toán");

                entity.Property(e => e.Payment)
                    .HasMaxLength(50)
                    .HasComment("Phương thức thanh toán");

                entity.Property(e => e.RecallDate).HasComment("Ngày thu hồi");

                entity.Property(e => e.ReceiveDate).HasComment("Ngày nhận");

                entity.Property(e => e.RefundDate).HasComment("Ngày hoàn trả");

                entity.Property(e => e.RentFee).HasComment("Phí thuê");

                entity.Property(e => e.RentId)
                    .HasColumnName("RentID")
                    .HasComment("Mã giao dịch thuê");

                entity.Property(e => e.SendDate).HasComment("Ngày gửi");

                entity.Property(e => e.TotalAmount).HasComment("Tổng tiền");

                entity.Property(e => e.TotalBook).HasComment("Tổng sách");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("Mã người dùng");

                entity.HasOne(d => d.FeeId1Navigation)
                    .WithMany(p => p.RentBillFeeId1Navigations)
                    .HasForeignKey(d => d.FeeId1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RentBill_Fee1");

                entity.HasOne(d => d.FeeId2Navigation)
                    .WithMany(p => p.RentBillFeeId2Navigations)
                    .HasForeignKey(d => d.FeeId2)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RentBill_Fee2");

                entity.HasOne(d => d.FeeId3Navigation)
                    .WithMany(p => p.RentBillFeeId3Navigations)
                    .HasForeignKey(d => d.FeeId3)
                    .HasConstraintName("FK_RentBill_Fee3");

                entity.HasOne(d => d.Rent)
                    .WithMany(p => p.RentBills)
                    .HasForeignKey(d => d.RentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RentBill_Rent");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RentBills)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RentBill_User");
            });

            modelBuilder.Entity<RentDetail>(entity =>
            {
                entity.ToTable("RentDetail");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã chi tiết giao dịch thuê");

                entity.Property(e => e.AfterStatusBook).HasComment("Trang thái sách sau giao dịch");

                entity.Property(e => e.BeforeStatusBook).HasComment("Trạng thái sách trước giao dịch");

                entity.Property(e => e.BookId)
                    .HasColumnName("BookID")
                    .HasComment("Mã sách");

                entity.Property(e => e.ExpiredDate).HasComment("Ngày hết hạn");

                entity.Property(e => e.Flag).HasComment("Cờ");

                entity.Property(e => e.RentId)
                    .HasColumnName("RentID")
                    .HasComment("Mã giao dịch thuê");

                entity.Property(e => e.RequestTime)
                    .HasColumnType("timestamp without time zone")
                    .HasComment("Thời gian yêu cầu");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Trạng thái chi tiết giao dịch");

                entity.Property(e => e.StorageStatusBook)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Trạng thái lưu trữ sách");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.RentDetails)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RentDetail_Book");

                entity.HasOne(d => d.Rent)
                    .WithMany(p => p.RentDetails)
                    .HasForeignKey(d => d.RentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RentDetail_Rent");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã vai trò");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Vai trò");
            });

            modelBuilder.Entity<ShipInfo>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("ShipInfo");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserID")
                    .HasComment("Mã người dùng");

                entity.Property(e => e.IsUpdate).HasComment("Đã cập nhật?");

                entity.Property(e => e.RecallIsFriday).HasComment("Thu hồi thứ 6");

                entity.Property(e => e.RecallIsMonday).HasComment("Thu hồi thứ 2");

                entity.Property(e => e.RecallIsWednesday).HasComment("Thu hồi thứ 4");

                entity.Property(e => e.ReceiveIsFriday).HasComment("Nhận thứ 6?");

                entity.Property(e => e.ReceiveIsMonday).HasComment("Nhận thứ 2?");

                entity.Property(e => e.ReceiveIsWednesday).HasComment("Nhận thứ 4?");

                entity.Property(e => e.RefundIsFriday).HasComment("Hoàn trả thứ 6");

                entity.Property(e => e.RefundIsMonday).HasComment("Hoàn trả thứ 2");

                entity.Property(e => e.RefundIsWednesday).HasComment("Hoàn trả thứ 4");

                entity.Property(e => e.SendIsFriday).HasComment("Gửi thứ 6?");

                entity.Property(e => e.SendIsMonday).HasComment("Gửi thứ 2?");

                entity.Property(e => e.SendIsWednesday).HasComment("Gửi thứ 4?");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.ShipInfo)
                    .HasForeignKey<ShipInfo>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShipInfo_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("Mã người dùng");

                entity.Property(e => e.AddressMain)
                    .IsRequired()
                    .HasComment("Địa chỉ chính");

                entity.Property(e => e.AddressSub1).HasComment("Địa chỉ phụ 1");

                entity.Property(e => e.AddressSub2).HasComment("Địa chỉ phụ 2");

                entity.Property(e => e.Age).HasComment("Tuổi");

                entity.Property(e => e.Avatar).HasComment("Ảnh đại diện");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Email");

                entity.Property(e => e.ForgotPasswordCode)
                    .HasMaxLength(10)
                    .HasComment("Mã quên mật khẩu");

                entity.Property(e => e.Fullname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Tên đầy đủ");

                entity.Property(e => e.IsActive).HasComment("Đang hoạt động?");

                entity.Property(e => e.IsVerify).HasComment("Đã xác thực?");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasComment("Mật khẩu");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasComment("Số điện thoại");

                entity.Property(e => e.RoleId)
                    .HasColumnName("RoleID")
                    .HasComment("Mã vai trò");

                entity.Property(e => e.VerificationToken)
                    .IsRequired()
                    .HasComment("Mã xác thực");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
