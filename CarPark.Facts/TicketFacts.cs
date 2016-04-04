using CarPark.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarPark.Facts
{
    public class TicketFacts
    {
        public class General
        {
            [Fact]
            public void BasicUsage()
            {
                // arrange
                Ticket t;

                t = new Ticket();
                t.PlateNo = "1707";
                t.DateIn = new DateTime(2016, 1, 1, 9, 0, 0); // 9am
                t.DateOut = DateTime.Parse("13:30"); // 1:30pm

                // act

                // assert
                Assert.Equal("1707", t.PlateNo);
                Assert.Equal(9, t.DateIn.Hour);
                Assert.Equal(13, t.DateOut.Value.Hour);
            }

            [Fact]
            public void NewTicket_HasNoDateOut()
            {
                var t = new Ticket();

                Assert.Null(t.DateOut);
            }
        }

        public class ParkingFeeProperty
        {
            [Fact]
            public void NewTicket_DontKnowParkingFee()
            {
                var t = new Ticket();
                t.DateIn = DateTime.Parse("9:00");
                t.DateOut = null;

                Assert.Null(t.ParkingFee);
            }

            [Fact]
            public void First15Minutes_Free()
            {
                //arrange
                var t = new Ticket();
                t.DateIn = DateTime.Parse("9:00");
                t.DateOut = DateTime.Parse("9:15");

                //act
                decimal? fee = t.ParkingFee;

                //assert
                Assert.Equal(0m, fee);
            }

            [Fact]
            public void WithInFirst3Hours_50Baht()
            {
                var t = new Ticket();
                t.DateIn = DateTime.Parse("9:00");
                t.DateOut = DateTime.Parse("9:15:01");

                var fee = t.ParkingFee;

                Assert.Equal(50m, fee);
            }

            [Fact]
            public void WithInFirst3HoursII_50Baht()
            {
                var t = new Ticket();
                t.DateIn = DateTime.Parse("9:00");
                t.DateOut = DateTime.Parse("12:11");

                var fee = t.ParkingFee;

                Assert.Equal(50m, fee);
            }

            [Fact]
            public void For6Hours_140Baht()
            {
                var t = new Ticket();
                t.DateIn = DateTime.Parse("9:00");
                t.DateOut = DateTime.Parse("15:00");

                var fee = t.ParkingFee;

                Assert.Equal(140m, fee);
            }

            [Fact]
            public void For6HoursExceed15Minutes_GetExtraHour()
            {
                var t = new Ticket();
                t.DateIn = DateTime.Parse("9:00");
                t.DateOut = DateTime.Parse("15:15:01");

                var fee = t.ParkingFee;

                Assert.Equal(170m, fee);
            }

            [Theory]
            [InlineData("9:00", "17:00", 200)]
            [InlineData("9:00", "18:00", 230)]
            [InlineData("9:00", "19:00", 260)]
            public void SamplingTests(string dateIn, string dateOut, decimal expectedFee)
            {
                var t = new Ticket();
                t.DateIn = DateTime.Parse(dateIn);
                t.DateOut = DateTime.Parse(dateOut);

                var fee = t.ParkingFee;

                Assert.Equal(expectedFee, fee);
            }

            [Fact]
            public void DateOutIsBeforeDateIn_ThrowsException()
            {
                var t = new Ticket();
                t.DateIn = DateTime.Parse("9:00");
                t.DateOut = DateTime.Parse("7:00");

                var ex = Assert.Throws<Exception>(() =>
                {
                    var fee = t.ParkingFee;
                });

                Assert.Equal("Invalid date", ex.Message);
            }
        }
    }
}
