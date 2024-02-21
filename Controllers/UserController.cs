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
                    .Select(r => r.ReviewID)
                    .FirstOrDefault();

                byte convertedRating = (byte)Math.Min(255, Math.Max(0, rating));
                if (existingReview.HasValue)
                {
                    // Eğer varsa, mevcut review'a ekle
                    var reviewToUpdate = context.TBLREVIEWs.Find(existingReview.Value);
                    if (reviewToUpdate != null)
                    {
                        reviewToUpdate.Comment = comment;
                        reviewToUpdate.Rating = convertedRating;
                    }
                }
                else
                {
                    // Yoksa, yeni bir review oluştur
                    var newReview = new TBLREVIEW
                    {
                        Comment = comment,
                        Rating = convertedRating
                    };

                    // Veritabanına kaydet ve ReservationID'yi güncelle
                    context.TBLREVIEWs.Add(newReview);
                    context.SaveChanges();

                    var reservationToUpdate = context.TBLRESERVATIONs.Find(reservationId);
                    if (reservationToUpdate != null)
                    {
                        reservationToUpdate.ReviewID = newReview.ReviewID;
                    }
                }

                // Değişiklikleri kaydet
                context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Hata durumunda
                Console.Error.WriteLine($"Değerlendirme gönderilirken hata oluştu: {ex.Message}");
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public ActionResult UpdateReviewStatus(int reservationId)
        {
            try
            {
                // reservationId ile ilgili rezervasyonu bulun
                var reservation = context.TBLRESERVATIONs.Find(reservationId);
                
                if (reservation != null)
                {
                    // ReviewStatus'u 0 olarak güncelle
                    reservation.ReviewStatus = false;

                    // Değişiklikleri kaydet
                    context.SaveChanges();

                    return Json(new { success = true, message = "ReviewStatus güncellendi." });
                }
                else
                {
                    return Json(new { success = false, message = "Rezervasyon bulunamadı." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }

    }
}