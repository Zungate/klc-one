namespace klc_one.Areas.FoodPlan.Models
{
    public class ResponseMessage
    {
        public int StatusCode { get; set; }
        public String Message { get; set; }

        public ResponseMessage(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
