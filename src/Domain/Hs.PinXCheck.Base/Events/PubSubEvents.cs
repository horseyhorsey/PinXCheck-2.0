using System;
using Prism.Events;
using System.Collections.Generic;

namespace Hs.PinXCheck.Base.Events
{
    public class MainMenuSelectedEvent : PubSubEvent<string>
    {

    }

    public class SystemSelected : PubSubEvent<string>
    {

    }

    public class DatabaseChanged : PubSubEvent<string>
    {

    }

    public class DatabaseUpdated : PubSubEvent<string>
    {

    }

    public class TableSelectedEvent : PubSubEvent<string>
    {

    }

    public class ShowDialogEvent : PubSubEvent<string>
    {

    }


    public class UpdatedUnusedTables : PubSubEvent<object>
    {

    }

    public class ReplaceTableEvent : PubSubEvent<string>
    {

    }

    public class ReplaceExecutableEvent : PubSubEvent<string>
    {

    }

    public class GetTableInfoEvent : PubSubEvent<string>
    {

    }

    public class AddToUnusedTablesEvent : PubSubEvent<string>
    {

    }

    public class DisableControlsEvent: PubSubEvent<bool>
    {

    }

    public class SetExtraTableOptionsEvent : PubSubEvent<Dictionary<string,bool>>
    {

    }

    public class FilterEvent : PubSubEvent<List<object>>
    {

    }

    public class DescriptionUpdatedEvent : PubSubEvent<string>
    {

    }

    public class RefreshMainDatabaseEvent : PubSubEvent<string>
    {

    }
}
