using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.AnalyticsReporting.v4.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using MaCode.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MaCode.Analytics
{
    public interface IGoogleAnalytics
    {
        (int pageViews, int userCount, int newUsers, int sessionsCount) GetReport(DateTime StartDate, DateTime EndDate);
        void Initialize(GoogleConnection dataModel, string viewId);
    }

    public class GoogleAnalytics : IGoogleAnalytics
    {
        public GoogleConnection cr { get; set; }
        public ServiceAccountCredential xCred { get; set; }


        public void Initialize(GoogleConnection dataModel, string viewId)
        {
            cr = dataModel;
            cr.ViewId = viewId;
            xCred = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(cr.client_email)
            {
                Scopes = new[] {
                    AnalyticsReportingService.Scope.Analytics
                }
            }.FromPrivateKey(cr.private_key));
        }
        public (int pageViews, int userCount, int newUsers, int sessionsCount) GetReport(DateTime StartDate, DateTime EndDate)
        {
            using (var svc = new AnalyticsReportingService(
                                new BaseClientService.Initializer
                                {
                                    HttpClientInitializer = xCred,
                                    ApplicationName = "Wandering Tales"
                                }))
            {
                List<ReportRequest> requests = new List<ReportRequest>();
                requests.Add(getPageViewsRequest(StartDate, EndDate));
                requests.Add(getUsersReport(StartDate, EndDate));
                requests.Add(getNewUsersReport(StartDate, EndDate));
                requests.Add(getSessionsReport(StartDate, EndDate));

                GetReportsRequest getReport = new GetReportsRequest() { ReportRequests = requests };


                GetReportsResponse response = svc.Reports.BatchGet(getReport).Execute();

                var pageViewsReportResult = response.Reports.First().Data;
                var siteUsersReportResult = response.Reports[1].Data;
                var newUsersReportResult = response.Reports[2].Data;
                var sessionsReportResult = response.Reports[3].Data;

                return (
                    pageViewsReportResult.GetTotal(),
                    siteUsersReportResult.GetTotal(),
                    newUsersReportResult.GetTotal(),
                    sessionsReportResult.GetTotal());
            }
        }


        private ReportRequest getPageViewsRequest(DateTime StartDate, DateTime EndDate)
        {
            DateRange dateRange = new DateRange() { StartDate = StartDate.GetReportDate(), EndDate = EndDate.GetReportDate() };
            Metric pageViews = new Metric { Expression = "ga:pageviews", Alias = "Pageviews" };

            //Dimension country = new Dimension { Name = "ga:country" };

            ReportRequest pageViewsReport = new ReportRequest
            {
                ViewId = cr.ViewId,
                DateRanges = new List<DateRange>() { dateRange },
                //Dimensions = new List<Dimension>() { country },
                Metrics = new List<Metric>() { pageViews }
            };
            return pageViewsReport;
        }
        private ReportRequest getUsersReport(DateTime StartDate, DateTime EndDate)
        {
            DateRange dateRange = new DateRange() { StartDate = StartDate.GetReportDate(), EndDate = EndDate.GetReportDate() }; Metric siteUsers = new Metric() { Expression = "ga:users", Alias = "Users" };
            ReportRequest siteUsersReport = new ReportRequest
            {
                ViewId = cr.ViewId,
                DateRanges = new List<DateRange>() { dateRange },
                Metrics = new List<Metric>() { siteUsers }
            };
            return siteUsersReport;
        }
        private ReportRequest getNewUsersReport(DateTime StartDate, DateTime EndDate)
        {
            DateRange dateRange = new DateRange() { StartDate = StartDate.GetReportDate(), EndDate = EndDate.GetReportDate() };
            Metric newUsersMetric = new Metric() { Expression = "ga:newUsers", Alias = "New Users" };

            ReportRequest newUsersReport = new ReportRequest
            {
                ViewId = cr.ViewId,
                DateRanges = new List<DateRange>() { dateRange },
                Metrics = new List<Metric>() { newUsersMetric }
            };
            return newUsersReport;
        }
        private ReportRequest getSessionsReport(DateTime StartDate, DateTime EndDate)
        {
            DateRange dateRange = new DateRange() { StartDate = StartDate.GetReportDate(), EndDate = EndDate.GetReportDate() };

            Metric sessions = new Metric() { Expression = "ga:sessions", Alias = "Sessions" };
            ReportRequest sessionsReport = new ReportRequest
            {
                ViewId = cr.ViewId,
                DateRanges = new List<DateRange>() { dateRange },
                Metrics = new List<Metric>() { sessions }
            };
            return sessionsReport;
        }
    }
}
