using System;
using System.Linq;
using System.Collections.Generic;
using Blogpost.Domain.Common;

namespace Blogpost.Domain.Entities
{
    public class Profile : AggregateRoot<Guid>
    {
        public string Name { get; private set; }

        public string Email { get; private set; }

        private readonly List<Post> _posts = new();
        public virtual IReadOnlyList<Post> Posts => _posts;

        private readonly List<Subscription> _subscriptions = new();
        public virtual IReadOnlyCollection<Subscription> Subscriptions => _subscriptions;

        protected Profile() { }

        public Profile(Guid id, string name, string email)
            : base(id)
        {
            Name = name;
            Email = email;
        }

        public void SubscribeTo(Profile profile)
        {
            if (profile == this)
                throw new InvalidOperationException("User cannot subscribe to himself");

            if (_subscriptions.Any(x => x.Profile == profile && x.Subscriber == this))
                throw new InvalidOperationException($"User is already subscribed to {profile.Id}");

            _subscriptions.Add(new Subscription(profile, this));
        }

        public void UnsubscribeFrom(Profile profile)
        {
            var subscription = _subscriptions.SingleOrDefault(x => x.Profile == profile);
            if (subscription is null)
            {
                throw new InvalidOperationException($"User is not subscribed to {profile.Id}");
            }

            _subscriptions.Remove(subscription);
        }
    }
}