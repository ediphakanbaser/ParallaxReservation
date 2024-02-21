using Parallax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Parallax.Helpers
{
    public static class TimeModelHelper
    {        
        public static TimeViewModel GetTimeModel(TBLPAGE pageModel)
        {
            TimeSpan workStartTime = pageModel.WorkStartTime;
            TimeSpan breakStartTime = pageModel.BreakStartTime;
            TimeSpan breakEndTime = pageModel.BreakEndTime;
            TimeSpan workEndTime = pageModel.WorkEndTime;
            
            string formattedWorkStartTime = FormatTimeSpan(workStartTime);
            string formattedBreakStartTime = FormatTimeSpan(breakStartTime);
            string formattedBreakEndTime = FormatTimeSpan(breakEndTime);
            string formattedWorkEndTime = FormatTimeSpan(workEndTime);
            
            TimeViewModel timeModel = new TimeViewModel
            {
                FormattedWorkStartTime = formattedWorkStartTime,
                FormattedBreakStartTime = formattedBreakStartTime,
                FormattedBreakEndTime = formattedBreakEndTime,
                FormattedWorkEndTime = formattedWorkEndTime
            };
            return timeModel;
        }

        public static string FormatTimeSpan(TimeSpan timeSpan)
        {
            DateTime today = DateTime.Today;
            DateTime timeModel = today.Add(timeSpan);
            return timeModel.ToString("HH:mm");
        }        
    }



}