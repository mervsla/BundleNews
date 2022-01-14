using BundleNews.Core.Converter;
using BundleNews.Core.Services.RSSService.Concrete;
using BundleNews.Data.Entities.Concrete;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace BundleNews.Core.RSSService
{
    public class RssFeedGenerator
    {

        private readonly string _rssLink;
        private readonly string _rssName;
        private readonly IHostEnvironment _environment;
        public RssFeedGenerator(string categoryLink, string categoryName, IHostEnvironment environment)
        {
            _rssLink = categoryLink;
            _rssName = categoryName;
            _environment = environment;
        }
        public bool RSSCreator()
        {
            bool result = false;
            Category _category = null;
            CategoryService categoryService = new CategoryService();
            NewsSourceService newsSourceService = new NewsSourceService();
            NewsContentService newsContentService = new NewsContentService();
            SyndicationFeed feed = null;

            try
            {
                using (var reader = XmlReader.Create(_rssLink))
                {
                    feed = SyndicationFeed.Load(reader);
                }
            }
            catch (Exception ex)
            { throw new Exception("Doğru bir RSS kodu giriniz.."); } // TODO: Deal with unavailable resource.
            //kateori getirme yanlış???????
            Category controlCategory = ControlCategory(categoryService);
            NewsSource controlSource = ControlSource(newsSourceService, feed);

            if (feed != null)
            {
                if (controlSource == null)
                {
                    int sourceResult = AddSource(newsSourceService, feed);
                    NewsSource _source = null;
                    if (sourceResult > 0)
                        _source = GetSource(newsSourceService, feed, _source);
                    else
                        throw new Exception("Kaynak eklenirken hata oluştu.");
                    int categoryResult = AddCategory(categoryService, _source);
                    if (categoryResult > 0)
                        _category = GetCategory(categoryService);
                    else
                        throw new Exception("Kategori eklenirken hata oluştu.");
                    int sourceContentResult = AddNewsContent(newsContentService, feed, _category);
                    if (sourceContentResult > 0)
                    {
                        result = true;
                    }
                    else
                        throw new Exception("İçerikler yüklenemedi");
                }
                else
                {
                    if (controlCategory == null)
                    {
                        int categoryResult = 0;
                        if (controlSource != null)
                            categoryResult = AddCategory(categoryService, controlSource);
                        else
                            throw new Exception("Kategori yanlış kaydedilmiştir.");


                        if (categoryResult > 0)
                            _category = GetCategory(categoryService);
                        else
                            throw new Exception("Kategori eklenirken hata oluştu.");

                        int sourceContentResult = AddNewsContent(newsContentService, feed, _category);
                        if (sourceContentResult > 0)
                        {
                            result = true;
                        }
                        else
                            throw new Exception("İçerikler yüklenemedi");
                    }
                    else
                        throw new Exception("Kategori kaydı mevcuttur");
                }




            }
            return result;
        }
        private int AddNewsContent(NewsContentService sourceContentService, SyndicationFeed feed, Category _category)
        {
            List<NewsContent> newsContentList = new List<NewsContent>();
            NewsContent sourceContent = null;
            string content = null;
            foreach (var element in feed.Items)
            {
                string image = null;


                if (element.Summary != null || element.Content != null)
                {
                    content = ContentSlice(element);

                }

                if (element.Links.Any(x => x.RelationshipType != null ? x.RelationshipType.Contains("enclosure") : false))
                {
                    image = (element.Links.Where(x => x.RelationshipType != null ? x.RelationshipType.Contains("enclosure") : false).Select(x => x.Uri.OriginalString.Trim().ToLower()).FirstOrDefault());
                }
                else if (element.ElementExtensions.Any(x => x.OuterName != null ? x.OuterName.Contains("image") : false))
                {
                    foreach (SyndicationElementExtension extension in element.ElementExtensions)
                    {
                        if (extension.OuterName == "image" || extension.OuterName == "ipimage")
                        {
                            XElement ele = extension.GetObject<XElement>();
                            image = ele.Value.Trim();
                        }

                    }
                }
                else
                    image = SliceImage(image, content);

                var imageSource = DownloadImage(image, ((element.Title.Text.Substring(0, 5)).Replace(" ", "")) + (Guid.NewGuid().ToString()), ImageFormat.Jpeg);

                sourceContent = new NewsContent()
                {
                    ContentInsertAt = element.PublishDate != null ? Convert.ToDateTime(new DateTime(element.PublishDate.Year, element.PublishDate.Month, element.PublishDate.Day, element.PublishDate.Hour, element.PublishDate.Minute, element.PublishDate.Second, element.PublishDate.Millisecond)) : DateTime.Now,
                    InsertedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CategoryId = _category.Id,
                    Description = content,
                    NewsContentId = element.Id,
                    IsDeleted = false,
                    imageURL = imageSource,
                    Title = element.Title.Text.Trim()
                };
                newsContentList.Add(sourceContent);
            }

            if (newsContentList.Count == 0 || sourceContent == null)
                throw new Exception("Bu linkte içerik bulunmamaktadır.");
            int sourceContentResult = sourceContentService.AddSourceContent(newsContentList);
            return sourceContentResult;
        }

        private static string ContentSlice(SyndicationItem element)
        {
            string content = element.Summary != null ? element.Summary.Text : ((TextSyndicationContent)element.Content).Text.Trim().ToString();
            if (content.Contains('<') && content.Contains('>'))
            {
                foreach (var item in content)
                {
                    if (item == '<')
                    {
                        int beginIndex = content.IndexOf(item);
                        int lastIndex = content.IndexOf('>');
                        content = content.Remove(beginIndex, (lastIndex - beginIndex) + 1);
                    }
                }
            }

            return content;
        }

        private static string SliceImage(string imageL, string content)
        {
            int imgurLBeginIndex = content.IndexOf("<img");

            if (imgurLBeginIndex != -1)
            {
                int imgUrlEndIndex = content.IndexOf(">", imgurLBeginIndex);
                string URL = content.Substring(imgurLBeginIndex, (imgUrlEndIndex - imgurLBeginIndex) + 1);
                string beginString = "src=";
                int imageBeginIndex = URL.IndexOf(beginString);
                imageBeginIndex += imageBeginIndex;

                int imageLastIndex = URL.IndexOf("\"", imageBeginIndex);
                imageL = URL.Substring(imageBeginIndex, (imageLastIndex - imageBeginIndex));
            }
            return imageL;
        }

        private Category GetCategory(CategoryService categoryService)
        {
            Category _category = categoryService.GetByRSSURL(_rssLink);
            if (_category == null)
                throw new Exception("Kategori eklendi fakat getirilirken hata oluştu");
            return _category;
        }
        private int AddCategory(CategoryService categoryService, NewsSource _source)
        {
            int categoryResult = 0;
            categoryResult = categoryService.AddCategory(_rssName, _rssLink, _source.Id);
            return categoryResult;
        }
        private NewsSource GetSource(NewsSourceService sourceService, SyndicationFeed feed, NewsSource _source)
        {
            if (feed.Links.Count != 0)
            {
                _source = sourceService.GetByURL(feed.Links[0].Uri.OriginalString, feed.Title.Text);
            }
            else if (!string.IsNullOrEmpty(feed.Id))
            {
                _source = sourceService.GetByURL(feed.Id, feed.Title.Text);
            }
            if (_source == null)
                throw new Exception("Kaynak eklendi fakat getirilirken hata oluştu");
            return _source;
        }
        private int AddSource(NewsSourceService sourceService, SyndicationFeed feed)
        {
            int sourceResult = 0;
            
            if (feed.Links.Count != 0)
            {
                sourceResult = sourceService.Add(feed.Title.Text.Trim(), feed.Links[0].Uri.OriginalString);
            }
            else
            {
                sourceResult = sourceService.Add(feed.Title.Text.Trim(), feed.Id);
            }

            return sourceResult;
        }

        private string DownloadImage(string imageURL, string fileName, ImageFormat imageFormat)
        {
            string imageSrc;
            try
            {
                string sImageFormat = "." + ((imageFormat.ToString()).ToLower());
                CharacterConverter converter = new CharacterConverter();
                fileName = converter.TurkishToEnglish(fileName);
                fileName = converter.RemovePunctuation(fileName);
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(imageURL);
                Bitmap bitmap; bitmap = new Bitmap(stream);

                if (bitmap != null)
                {

                    System.IO.FileStream fs = System.IO.File.Open(Path.Combine(_environment.ContentRootPath + @"/wwwroot/images", fileName + sImageFormat), FileMode.Create);
                    bitmap.Save(fs, imageFormat);
                    fs.Close();


                }

                stream.Flush();
                stream.Close();
                client.Dispose();
                imageSrc = fileName + sImageFormat;

            }
            catch (Exception ex)
            {
                imageSrc = null;
            }

            return imageSrc;
        }

        private Category ControlCategory(CategoryService categoryService)
        {
            return categoryService.GetByRSSURL(_rssLink);
        }
        private NewsSource ControlSource(NewsSourceService sourceService, SyndicationFeed feed)
        {
            NewsSource controlSource = null;
            if (feed.Links.Count != 0)
            {
                controlSource = sourceService.GetByURL(feed.Links[0].Uri.OriginalString, feed.Title.Text);
            }
            else if (!string.IsNullOrEmpty(feed.Id))
            {
                controlSource = sourceService.GetByURL(feed.Id, feed.Title.Text);
            }

            return controlSource;
        }
    }
}
