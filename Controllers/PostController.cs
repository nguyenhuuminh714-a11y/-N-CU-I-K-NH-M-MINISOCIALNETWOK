using ĐỀ_ÁN_CUỐI_KỲ_NHÓM_MINISOCIALNETWOK.Models;
using ĐỀ_ÁN_CUỐI_KỲ_NHÓM_MINISOCIALNETWOK.Models.ViewModle;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ĐỀ_ÁN_CUỐI_KỲ_NHÓM_MINISOCIALNETWOK.Controllers
{
    public class PostController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // Lấy ID người dùng đang đăng nhập từ Session
        private int GetCurrentUserId()
        {
            return Session["UserId"] != null ? (int)Session["UserId"] : 1; // Giả lập UserId = 1 nếu chưa đăng nhập
        }
        // GET: Post
        public ActionResult Newsfeed(int? page)
        {
            int currentUserId = GetCurrentUserId();
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            // Lấy danh sách ID của những người mà user này đang Follow
            var followingIds = db.Follows
                                 .Where(f => f.FollowerId == currentUserId)
                                 .Select(f => f.FollowingId)
                                 .ToList();

            // Luồng bài đăng: Bao gồm bài viết của BẢN THÂN + những người ĐANG FOLLOW
            var postsQuery = db.Posts
                               .Where(p => p.UserId == currentUserId || followingIds.Contains(p.UserId))
                               .OrderByDescending(p => p.CreatedAt);

            var viewModel = new NewsfeedVM
            {
                Posts = postsQuery.ToPagedList(pageNumber, pageSize),
                CurrentUserId = currentUserId
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost(string Content, HttpPostedFileBase ImageFile)
        {
            int currentUserId = GetCurrentUserId();

            if (!string.IsNullOrWhiteSpace(Content))
            {
                var post = new Post
                {
                    UserId = currentUserId,
                    Content = Content,
                    CreatedAt = DateTime.Now
                };

                // Xử lý Upload hình ảnh bài viết
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName)
                                    + "_" + DateTime.Now.Ticks
                                    + Path.GetExtension(ImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images/Posts/"), fileName);
                    ImageFile.SaveAs(path);
                    post.ImageUrl = "/Images/Posts/" + fileName;
                }

                db.Posts.Add(post);
                db.SaveChanges();
            }

            return RedirectToAction("Newsfeed");
        }

        // 3. XÓA BÀI ĐĂNG (Ràng buộc nghiệp vụ: Chỉ xóa bài của chính mình)
        [HttpPost]
        public ActionResult DeletePost(int id)
        {
            int currentUserId = GetCurrentUserId();
            var post = db.Posts.Find(id);

            if (post != null)
            {
                // KIỂM TRA QUYỀN SỞ HỮU
                if (post.UserId == currentUserId)
                {
                    db.Posts.Remove(post);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Đã xóa bài viết thành công!" });
                }
                return Json(new { success = false, message = "Bạn không có quyền xóa bài viết này!" });
            }

            return Json(new { success = false, message = "Bài viết không tồn tại!" });
        }

        // 4. THẢ LIKE / UNLIKE (AJAX)
        [HttpPost]
        public ActionResult ToggleLike(int postId)
        {
            int currentUserId = GetCurrentUserId();
            var existingLike = db.Likes.FirstOrDefault(l => l.PostId == postId && l.UserId == currentUserId);

            bool isLiked;
            if (existingLike != null)
            {
                db.Likes.Remove(existingLike);
                isLiked = false;
            }
            else
            {
                db.Likes.Add(new Like { PostId = postId, UserId = currentUserId, CreatedAt = DateTime.Now });
                isLiked = true;
            }

            db.SaveChanges();
            int totalLikes = db.Likes.Count(l => l.PostId == postId);

            return Json(new { success = true, isLiked = isLiked, totalLikes = totalLikes });
        }
    }
}