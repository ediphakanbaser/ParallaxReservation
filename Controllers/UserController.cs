using Parallax.Helpers;
using Parallax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Parallax.Controllers
{
    public class UserController : Controller
    {
        private readonly ParallaxContext context;

        public UserController()
        {
            context = new ParallaxContext();
        }
        public ActionResult Testimonial()
        {
            TBLPAGE pageModel = context.TBLPAGEs.FirstOrDefault();
            TimeViewModel timeModel = TimeModelHelper.GetTimeModel(pageModel);

            string usernameFromSession = (string)Session["UsernameSS"];

            // Çift tırnakları kaldırmak için Replace fonksiyonu kullanılıyor
            string cleanedUsername = usernameFromSession.Replace("\"", "");

            List<TBLRESERVATION> reservationModels = context.TBLRESERVATIONs
       .Where(r => r.TBLUSER.Username == cleanedUsername)
       .ToList();

            TestimonialsViewModel testimonialViewModel = new TestimonialsViewModel()
            {
                PageModel = pageModel,
                ReservationModels = reservationModels,
                TimeModel = timeModel
            };


            return View(testimonialViewModel);
        }

        [HttpPost]
        public ActionResult SubmitReview(int reservationId, string comment, int rating)
        {
            try
            {
                // ReservationID'ye ait bir review zaten var mı kontrol et
                var existingReview = context.TBLRESERVATIONs
                    .Where(r => r.ReservationID == reservationId)
                    .Select(r => new { r.ReviewID, r.ReviewStatus })
                    .FirstOrDefault();

                if (existingReview != null && existingReview.ReviewID.HasValue)
                {
                    // Eğer varsa, mevcut review'a ekle
                    var reviewToUpdate = context.TBLREVIEWs.Find(existingReview.ReviewID.Value);
                    if (reviewToUpdate != null)
                    {
                        reviewToUpdate.Comment = comment;
                        reviewToUpdate.Rating = (byte)Math.Min(255, Math.Max(0, rating));
                    }
                }
                else
                {
                    // Yoksa, yeni bir review oluştur
                    var newReview = new TBLREVIEW
                    {
                        Comment = comment,
                        Rating = (byte)Math.Min(255, Math.Max(0, rating))
                    };

                    // Veritabanına kaydet
                    context.TBLREVIEWs.Add(newReview);
                    context.SaveChanges();

                    // ReservationID'yi güncelle
                    var reservationToUpdate = context.TBLRESERVATIONs.Find(reservationId);
                    if (reservationToUpdate != null)
                    {
                        reservationToUpdate.ReviewID = newReview.ReviewID;
                        reservationToUpdate.ReviewStatus = false;
                    }
                }

                // Değişiklikleri kaydet
                context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Değerlendirme gönderilirken hata oluştu: {ex.Message}");
                return Json(new { success = false });
            }
        }
    }
}