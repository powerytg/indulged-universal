
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

using Indulged.API.Networking;
using Indulged.UI.Models;
using System;

namespace Indulged.UI.Group.Sections
{
    public sealed partial class GroupDiscussionSection : GroupSectionBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GroupDiscussionSection()
        {
            this.InitializeComponent();
        }

        protected async override void OnGroupChanged()
        {
            base.OnGroupChanged();

            if (Group == null)
            {
                return;
            }

            // Load topic list
            var ds = new TopicCollection();
            TopicListView.ItemsSource = ds;
            ds.Group = Group;

            await ds.LoadMoreItemsAsync((uint)APIService.PerPage);
        }

        public override void AddEventListeners()
        {
            base.AddEventListeners();
        }

        public override void RemoveEventListeners()
        {
            base.RemoveEventListeners();
        }
    }
}
