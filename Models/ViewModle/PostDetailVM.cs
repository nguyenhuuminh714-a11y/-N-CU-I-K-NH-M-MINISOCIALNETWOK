using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ĐỀ_ÁN_CUỐI_KỲ_NHÓM_MINISOCIALNETWOK.Models.ViewModle
{
    public class PostDetailVM
    {
        public Post Post { get; set; }
        public List<CssComment> Comments { get; set; }
        public int LikeCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
        public string NewCommentContent { get; set; }
    }
}