﻿using System;
using System.IO;
using Telerik.Sitefinity.Frontend.TestUI.Arrangements.MvcWidgets;
using Telerik.Sitefinity.Frontend.TestUtilities;
using Telerik.Sitefinity.TestUI.Arrangements.Framework;
using Telerik.Sitefinity.TestUI.Arrangements.Framework.Attributes;
using Telerik.Sitefinity.TestUtilities.CommonOperations;

namespace Telerik.Sitefinity.Frontend.TestUI.Arrangements
{
    /// <summary>
    /// MvcWidgetDefaultFeatherDesigner arragement.
    /// </summary>
    public class MvcSelectorTest : ITestArrangement
    {
        [ServerSetUp]
        public void SetUp()
        {
            for (int i = 0; i < 3; i++)
            {
                ServerOperations.News().CreatePublishedNewsItem(newsTitle: NewsItemTitle + i, newsContent: NewsItemContent + i, author: NewsItemAuthor + i);                
            }
            
            for (int i = 0; i < 4; i++)
            {
                ServerOperations.ContentBlocks().CreateContentBlock(ContentBlockTitle + i, ContentBlockContent + i);
            }

            Guid pageId = ServerOperations.Pages().CreatePage(PageName);
        
            var assembly = FileInjectHelper.GetArrangementsAssembly();

            ////  inject DesignerView.Selector.cshtml
            Stream source = assembly.GetManifestResourceStream(FileResource);

            var viewPath = Path.Combine("MVC", "Views", "DummyText", DesignerViewFileName);

            string filePath = FileInjectHelper.GetDestinationFilePath(viewPath);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            Stream destination = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            FileInjectHelper.CopyStream(source, destination);
            source.Close();
            destination.Close();

            ////  inject DesignerView.Selector.json
            Stream sourceJson = assembly.GetManifestResourceStream(FileResourceJson);
            var jsonPath = Path.Combine("MVC", "Views", "DummyText", JsonFileName);

            string filePathJson = FileInjectHelper.GetDestinationFilePath(jsonPath);
            Directory.CreateDirectory(Path.GetDirectoryName(filePathJson));
            Stream destinationJson = new FileStream(filePathJson, FileMode.Create, FileAccess.Write);

            FileInjectHelper.CopyStream(sourceJson, destinationJson);
            sourceJson.Close();
            destinationJson.Close();

            ////  inject designerview-selector.js
            Stream sourceController = assembly.GetManifestResourceStream(ControllerFileResource);
            var controllerPath = Path.Combine("MVC", "Scripts", "DummyText", ControllerFileName);

            string controllerFilePath = FileInjectHelper.GetDestinationFilePath(controllerPath);
            Directory.CreateDirectory(Path.GetDirectoryName(controllerFilePath));
            Stream destinationController = new FileStream(controllerFilePath, FileMode.Create, FileAccess.Write);

            FileInjectHelper.CopyStream(sourceController, destinationController);
            
            sourceController.Close();
            destinationController.Close();            

            ServerOperations.Widgets().AddMvcWidgetToPage(pageId, typeof(DummyTextController).FullName, WidgetCaption);
        }

        [ServerTearDown]
        public void TearDown()
        {
            ServerOperations.Pages().DeleteAllPages();
            ServerOperations.News().DeleteAllNews();
            ServerOperations.ContentBlocks().DeleteAllContentBlocks();

            var path = Path.Combine("MVC", "Views", "DummyText", DesignerViewFileName);
            string filePath = FileInjectHelper.GetDestinationFilePath(path);
            File.Delete(filePath);

            var jsonPath = Path.Combine("MVC", "Views", "DummyText", JsonFileName);
            string filePathJson = FileInjectHelper.GetDestinationFilePath(jsonPath);
            File.Delete(filePathJson);

            var controllerPath = Path.Combine("MVC", "Scripts", "DummyText", ControllerFileName);
            string controllerFilePath = FileInjectHelper.GetDestinationFilePath(controllerPath);
            File.Delete(controllerFilePath);
        }

        private const string FileResource = "Telerik.Sitefinity.Frontend.TestUI.Arrangements.Data.DesignerView.Selector.cshtml";
        private const string FileResourceJson = "Telerik.Sitefinity.Frontend.TestUI.Arrangements.Data.DesignerView.Selector.json";
        private const string ControllerFileResource = "Telerik.Sitefinity.Frontend.TestUI.Arrangements.Data.designerview-selector.js";

        private const string DesignerViewFileName = "DesignerView.Selector.cshtml";
        private const string JsonFileName = "DesignerView.Selector.json";
        private const string ControllerFileName = "designerview-selector.js";

        private const string PageName = "FeatherPage";
        private const string WidgetCaption = "SelectorWidget";

        private const string NewsItemTitle = "News Item Title";
        private const string NewsItemContent = "This is a news item.";
        private const string NewsItemAuthor = "NewsWriter";

        private const string ContentBlockTitle = "Content Block Title";
        private const string ContentBlockContent = "Content block content!";
    }
}
