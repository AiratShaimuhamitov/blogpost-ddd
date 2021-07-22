using System;
using Domain.UnitTests.Common;
using Blogpost.Domain.Entities;
using Xunit;

namespace Domain.UnitTests.Entities
{
    public class ProfileTests
    {
        [Fact]
        public void Subscribe_To_Another_Profile_Is_Succeed()
        {
            // arrange
            var sut = new Profile(Guid.NewGuid(), "Pavel", "pavel@gmail.com");
            var secondProfile = new Profile(Guid.NewGuid(), "Ivan", "ivan@gmail.com");

            // act
            sut.SubscribeTo(secondProfile);

            // assert
            Assert.Equal(1, sut.Subscriptions.Count);
        }

        [Fact]
        public void Unsubscribe_From_Another_Profile_Is_Succeed()
        {
            // arrange
            var sut = new Profile(Guid.NewGuid(), "Pavel", "pavel@gmail.com");
            var secondProfile = new Profile(Guid.NewGuid(), "Ivan", "ivan@gmail.com");
            sut.SubscribeTo(secondProfile);

            // act
            sut.UnsubscribeFrom(secondProfile);

            // assert
            Assert.Equal(0, sut.Subscriptions.Count);
        }

        [Fact]
        public void Subscribing_To_Already_Subscribed_Profile_Throws_An_Exception()
        {
            // arrange
            var sut = new Profile(Guid.NewGuid(), "Pavel", "pavel@gmail.com");
            var secondProfile = new Profile(Guid.NewGuid(), "Ivan", "ivan@gmail.com");
            sut.SubscribeTo(secondProfile);

            // act && assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                sut.SubscribeTo(secondProfile);
            });
        }

        [Fact]
        public void Subscribing_To_Himself_Throws_An_Exception()
        {
            // arrange
            var sut = new Profile(Guid.NewGuid(), "Pavel", "pavel@gmail.com");

            // act && assert
            Assert.Throws<InvalidOperationException>(() => { sut.SubscribeTo(sut); });
        }
    }
}