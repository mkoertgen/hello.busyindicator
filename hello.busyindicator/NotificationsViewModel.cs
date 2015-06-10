using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;

namespace hello.busyindicator
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class NotificationsViewModel 
        : Screen
        , INotificationsViewModel
    {
        public NotificationsViewModel(IEventAggregator events)
        {
            if (events == null) throw new ArgumentNullException(nameof(events));
            events.Subscribe(this);
        }

        private readonly BindableCollection<string> _items = new BindableCollection<string>();
        private string _lastItem;
        private string _selectedItem;
        private int _maxItems = 5;
        private bool _itemsVisible;

        private NotificationsView _view;

        public IEnumerable<string> Items
        {
            get { return _items; }
            set
            {
                _items.Clear();
                if (value != null)
                    _items.AddRange(value);
                SelectedItem = _items.FirstOrDefault();
            }
        }

        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange();
            }
        }

        public string LastItem
        {
            get { return _lastItem; }
            set
            {
                if (Equals(value, _lastItem)) return;
                _lastItem = value;
                NotifyOfPropertyChange();
            }
        }

        public int MaxItems
        {
            get { return _maxItems; }
            set
            {
                if (value == _maxItems) return;
                if (_maxItems < 1) throw new ArgumentOutOfRangeException(nameof(value), "MaxItems must be a positive number");
                _maxItems = value;
                NotifyOfPropertyChange();
            }
        }

        public bool ItemsVisible
        {
            get { return _itemsVisible; }
            set
            {
                if (value.Equals(_itemsVisible)) return;
                _itemsVisible = value;
                NotifyOfPropertyChange();

                if (!_itemsVisible) return;

                // scroll to newest item
                SelectedItem = LastItem = _items.FirstOrDefault();
                if (SelectedItem != null)
                    _view?.Items.ScrollIntoView(SelectedItem);
            }
        }

        public void Add(string notification)
        {
            _items.Insert(0, notification);
            while (_items.Count > MaxItems)
                _items.RemoveAt(_items.Count -1);
            SelectedItem = LastItem = _items.First();
        }

        protected override void OnViewAttached(object view, object context)
        {
            _view = view as NotificationsView;
        }

        public void Handle(NotificationEvent message)
        {
            Add(message.Notification);
        }
    }
}