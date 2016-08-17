// Class1.cs
//

using ClientUI.ViewModel;
using jQueryApi;
using Slick;
using SparkleXrm;
using SparkleXrm.GridEditor;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xrm;
using Xrm.Sdk;


namespace ClientUI.View
{

    public static class ContactView
    {
        public static ContactViewModel vm;
        public static Grid contactsGrid;
        public static UserSettings currentUserSettings;

        [PreserveCase]
        public static void Init()
        {
            PageEx.MajorVersion = 2013; // Use the CRM2013/2015 styles

            Guid accountId = new Guid(ParentPage.Data.Entity.GetId());
            string logicalName = ParentPage.Data.Entity.GetEntityName();



            vm = new ContactViewModel(new EntityReference(accountId, logicalName,null)) ;
            currentUserSettings = OrganizationServiceProxy.GetUserSettings();

            List<Column> columns = GridDataViewBinder.ParseLayout(String.Format("{0}, firstname, 250, {1}, lastname, 250, {2}, creditlimit, 250, {3}, preferredcontactmethodcode, 250", ResourceStrings.FirstName, ResourceStrings.LastName, ResourceStrings.CreditLimit, ResourceStrings.PreferredContactType));
            

            GridDataViewBinder contactsDataBinder = new GridDataViewBinder();

            //List<Column> columns = GridDataViewBinder.ParseLayout(String.Format("{0}, firstname, 250, {1}, lastname, 250, {2}, preferredcontactmethodcode, 250, {3}, creditlimit, 250",ResourceStrings.FirstName, ResourceStrings.LastName, ResourceStrings.PreferredContactType, ResourceStrings.CreditLimit));

            GridDataViewBinder.AddEditIndicatorColumn(columns);

            

            
            
            // Grid Editor 
            XrmTextEditor.BindColumn(columns[0]);
            XrmTextEditor.BindColumn(columns[1]);

            //XrmOptionSetEditor.BindColumn(columns[2], "contact", "preferredcontactmethodcode", false);
            XrmMoneyEditor.BindColumn(columns[2], -1000, 1000);
            //XrmOptionSetEditor.BindColumn(columns[4], "contact", "preferredcontactmethodcode", false);
            XrmOptionSetEditor.BindColumn(columns[3],"contact","preferredcontactmethodcode", false);

            contactsGrid = contactsDataBinder.DataBindXrmGrid(vm.Contacts, columns, "container", "pager", true, false);
            
            contactsDataBinder.AddCheckBoxSelectColumn = false;
            contactsDataBinder.BindClickHandler(contactsGrid);
            
            
            ViewBase.RegisterViewModel(vm);



            jQuery.Window.Resize(OnResize);
            jQuery.OnDocumentReady(delegate()
            {
                vm.Search();
            });
            int lcid = OrganizationServiceProxy.GetUserSettings().UILanguageId.Value;
            /*LocalisedContentLoader.LoadContent("dev1_/js/Res.metadata.js", lcid, delegate() {
                ViewBase.RegisterViewModel(vm);
            

            
            jQuery.Window.Resize(OnResize);
            jQuery.OnDocumentReady(delegate()
            {
                vm.Search();
            });
            });*/
        }

        private static void OnResize(jQueryEvent e)
        {
            int height = jQuery.Window.GetHeight();
            int width = jQuery.Window.GetWidth();

            jQuery.Select("#container").Height(height - 64).Width(width - 1);
            contactsGrid.ResizeCanvas();
        }


    }
}
