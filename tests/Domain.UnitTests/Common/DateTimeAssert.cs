using System;

namespace Domain.UnitTests.Common
{
    public static class DateTimeAssert
    {
        public static void AreEqual(DateTime? expectedDate, DateTime? actualDate, TimeSpan maximumDelta)
        {
            if (expectedDate == null && actualDate == null)
            {
                return;
            }

            if (expectedDate == null)
            {
                throw new NullReferenceException("The expected date was null");
            }

            if (actualDate == null)
            {
                throw new NullReferenceException("The actual date was null");
            }
            
            var totalSecondsDifference = Math.Abs(((DateTime)actualDate - (DateTime)expectedDate).TotalSeconds);
            if (totalSecondsDifference > maximumDelta.TotalSeconds)
            {
                throw new Exception(
                    $"Expected Date: {expectedDate}, Actual Date: {actualDate} \nExpected Delta: {maximumDelta}, Actual Delta in seconds- {totalSecondsDifference}");
            }
        }
    }
}