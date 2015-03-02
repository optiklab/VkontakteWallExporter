using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WallExporter.Api;
using WallExporter.Infrastructure;
using WallExporter.Json;

namespace WallExporter
{
    class Program
    {
        #region Constants

        private const string VKONTAKTE_ID = "";
        private const string FILE_PATH = "C:\\wall.md";
        private const int MAX_RECORDS = 10000;

        #endregion

        #region Fields

        private static int _offset = 0;
        private static int _count = 100;
        private static List<WallPost> _list = new List<WallPost>();
        private static object _lock = new object();
        private static bool _finished = false;
        
        #endregion

        #region Methods

        static void Main(string[] args)
        {
            Thread thread = new Thread(_Start);
            thread.IsBackground = true;
            thread.Start();

            int counter = 0;

            while (!_finished)
            {
                if (counter == 0)
                {
                    Console.Clear();
                    Console.WriteLine(@"\ Loading");
                    ++counter;
                }
                else if (counter == 1)
                {
                    Console.Clear();
                    Console.WriteLine(@"| Loading");
                    ++counter;
                }
                else if (counter == 2)
                {
                    Console.Clear();
                    Console.WriteLine(@"/ Loading");
                    ++counter;
                }
                else if (counter == 3)
                {
                    Console.Clear();
                    Console.WriteLine(@"- Loading");
                    counter = 0;
                }
                Thread.Sleep(200);
            }

            Console.WriteLine(@"Finished. Wall exported to: D:\wall.md");
            Console.ReadKey();
        }

        /// <summary>
        /// Method downloads all Wall Posts (100 posts per request) for specified Id and save it into simple text file.
        /// </summary>
        private static void _Start()
        {
            int postsCount = 0;

            WallGet op = new WallGet(VKONTAKTE_ID, "", _offset, _count, "", info =>
            {
                try
                {
                    postsCount = info.Count;

                    lock (_lock)
                    {
                        _list.AddRange(info);
                        _offset += _count;
                    }

                    if (postsCount > 0 && _offset < MAX_RECORDS)
                    {
                        _Start();
                    }
                    else
                    {
                        using (StreamWriter sw = new StreamWriter(FILE_PATH, false))
                        {
                            foreach (var item in _list)
                            {                                
                                try
                                {
                                    _WritePostToFile(sw, item);
                                }
                                catch
                                {

                                }
                            }
                        }

                        _finished = true;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("WallGet failed: " + ex.Message);
                }
            });
            op.Execute();
        }

        private static void _WritePostToFile(StreamWriter sw, WallPost item)
        {
            sw.WriteLine("");
            sw.WriteLine("");
            // Date
            string date = CommonHelper.GetFormattedDate(item.date);
            sw.Write(date + "\n");
            for (int i = 0; i < date.Length; i++)
            {
                sw.Write("-");
            }

            // Text
            if (!String.IsNullOrEmpty(item.text))
            {
                sw.WriteLine("");
                sw.Write("**");
                sw.Write(item.text);
                sw.Write("**\n");
            }

            if (!String.IsNullOrEmpty(item.post_type))
            {
                sw.WriteLine(item.post_type);
            }

            sw.WriteLine("owner_id: " + item.owner_id);
            sw.WriteLine("id: " + item.id);
            sw.WriteLine("signer_id: " + item.signer_id);
            sw.WriteLine("reply_post_id: " + item.reply_post_id);
            sw.WriteLine("reply_owner_id: " + item.reply_owner_id);
            sw.WriteLine("from_id: " + item.from_id);
            sw.WriteLine("friends_only: " + item.friends_only);

            if (item.reposts != null)
            {
                sw.WriteLine("reposts.count: " + item.reposts.count);
                sw.WriteLine("reposts.user_reposted: " + item.reposts.user_reposted);
            }

            if (item.comments != null)
            {
                sw.WriteLine("comments.count: " + item.comments.count);
                sw.WriteLine("comments.can_post: " + item.comments.can_post);
            }

            if (item.post_source != null)
            {
                sw.WriteLine("post_source.type: " + item.post_source.type);
                sw.WriteLine("post_source.platform: " + item.post_source.platform);
                sw.WriteLine("post_source.data: " + item.post_source.data);
            }

            if (item.likes != null)
            {
                sw.WriteLine("likes.count: " + item.likes.count);
                sw.WriteLine("likes.can_like: " + item.likes.can_like);
                sw.WriteLine("likes.can_publish: " + item.likes.can_publish);
                sw.WriteLine("likes.user_likes: " + item.likes.user_likes);
            }

            if (item.geo != null)
            {
                sw.WriteLine("geo.type: " + item.geo.type);
                sw.WriteLine("geo.coordinates: " + item.geo.coordinates);

                if (item.geo.place != null)
                {
                    sw.WriteLine("geo.city: " + item.geo.place.city + " " + item.geo.place.country + " " + item.geo.place.address);
                }
            }

            sw.WriteLine("");
            if (item.attachments != null && item.attachments.Count > 0)
            {
                for (int i = 0; i < item.attachments.Count; i++)
                {
                    sw.Write("*");

                    switch (item.attachments[i].type)
                    {
                        case "photo":
                            sw.Write("фотография из альбома");
                            break;
                        case "posted_photo":
                            sw.Write("фотография, загруженная напрямую с компьютера пользователя");
                            break;
                        case "video":
                            sw.Write("видеозапись");
                            break;
                        case "audio":
                            sw.Write("аудиозапись");
                            break;
                        case "doc":
                            sw.Write("документ");
                            break;
                        case "graffiti":
                            sw.Write("граффити");
                            break;
                        case "link":
                            sw.Write("ссылка на web-страницу");
                            break;
                        case "note":
                            sw.Write("заметка");
                            break;
                        case "app":
                            sw.Write("изображение, загруженное сторонним приложением");
                            break;
                        case "poll":
                            sw.Write("опрос");
                            break;
                        case "page":
                            sw.Write("вики-страница");
                            break;
                        case "album":
                            sw.Write("альбом с фотографиями");
                            break;
                        case "photos_list":
                            sw.Write("список фотографий, размещенных в одном посте. Количество фотографий может превышать допустимое количество аттачей");
                            break;
                        default:
                            sw.Write("Unknown");
                            break;
                    }

                    sw.Write("*\n");

                    if (item.attachments[i].audio != null)
                    {
                        sw.WriteLine("id: " + item.attachments[i].audio.id);
                        sw.WriteLine("album_id: " + item.attachments[i].audio.album_id);
                        sw.WriteLine("artist: " + (item.attachments[i].audio.artist ?? ""));
                        sw.WriteLine("duration: " + item.attachments[i].audio.duration);
                        sw.WriteLine("genre_id: " + item.attachments[i].audio.genre_id);
                        sw.WriteLine("lyrics_id: " + item.attachments[i].audio.lyrics_id);
                        sw.WriteLine("owner_id: " + item.attachments[i].audio.owner_id);
                        sw.WriteLine("title: " + (item.attachments[i].audio.title ?? ""));
                        sw.WriteLine("url: " + (item.attachments[i].audio.url ?? ""));
                    }
                    else if (item.attachments[i].doc != null)
                    {
                        sw.WriteLine("id: " + item.attachments[i].doc.id);
                        sw.WriteLine("ext: " + (item.attachments[i].doc.ext ?? ""));
                        sw.WriteLine("owner_id: " + item.attachments[i].doc.owner_id);
                        sw.WriteLine("photo_100: " + (item.attachments[i].doc.photo_100 ?? ""));
                        sw.WriteLine("photo_130: " + (item.attachments[i].doc.photo_130 ?? ""));
                        sw.WriteLine("size: " + item.attachments[i].doc.size);
                        sw.WriteLine("title: " + (item.attachments[i].doc.title ?? ""));
                        sw.WriteLine("url: " + (item.attachments[i].doc.url ?? ""));
                    }
                    else if (item.attachments[i].link != null)
                    {
                        sw.WriteLine("title: " +( item.attachments[i].link.title ?? ""));
                        sw.WriteLine("image_src: " + (item.attachments[i].link.image_src ?? ""));
                        sw.WriteLine("description: " + (item.attachments[i].link.description ?? ""));
                        sw.WriteLine("url: " + (item.attachments[i].link.url ?? ""));
                    }
                    else if (item.attachments[i].note != null)
                    {
                        sw.WriteLine("id: " + item.attachments[i].note.id);
                        sw.WriteLine("title: " + (item.attachments[i].note.title ?? ""));
                        sw.WriteLine("text: " + (item.attachments[i].note.text ?? ""));
                        sw.WriteLine("view_url: " + (item.attachments[i].note.view_url ?? ""));
                        sw.WriteLine("owner_id: " + item.attachments[i].note.owner_id);
                        sw.WriteLine("comments: " + item.attachments[i].note.comments);
                        sw.WriteLine("read_comments: " + item.attachments[i].note.read_comments);
                        sw.WriteLine("user_id: " + item.attachments[i].note.user_id);
                    }
                    else if (item.attachments[i].Page != null)
                    {
                        sw.WriteLine("id: " + item.attachments[i].Page.id);
                        sw.WriteLine("created: " + item.attachments[i].Page.created);
                        sw.WriteLine("creator_id: " + item.attachments[i].Page.creator_id);
                        sw.WriteLine("html: " + (item.attachments[i].Page.html ?? ""));
                        sw.WriteLine("parent: " + (item.attachments[i].Page.parent ?? ""));
                        sw.WriteLine("parent2: " + (item.attachments[i].Page.parent2 ?? ""));
                        sw.WriteLine("source: " + (item.attachments[i].Page.source ?? ""));
                        sw.WriteLine("title: " + (item.attachments[i].Page.title ?? ""));
                        sw.WriteLine("view_url: " + (item.attachments[i].Page.view_url ?? ""));
                        sw.WriteLine("views: " + item.attachments[i].Page.views);
                        sw.WriteLine("edited: " + item.attachments[i].Page.edited);
                        sw.WriteLine("editor_id: " + item.attachments[i].Page.editor_id);
                        sw.WriteLine("group_id: " + item.attachments[i].Page.group_id);
                        sw.WriteLine("current_user_can_edit: " + item.attachments[i].Page.current_user_can_edit);
                        sw.WriteLine("current_user_can_edit_access: " + item.attachments[i].Page.current_user_can_edit_access);
                        sw.WriteLine("who_can_edit: " + item.attachments[i].Page.who_can_edit);
                        sw.WriteLine("who_can_view: " + item.attachments[i].Page.who_can_view);

                    }
                    else if (item.attachments[i].photo != null)
                    {
                        sw.WriteLine("id: " + item.attachments[i].photo.id);
                        sw.WriteLine("album_id: " + item.attachments[i].photo.album_id);
                        sw.WriteLine("date: " + CommonHelper.GetFormattedDate(item.attachments[i].photo.date));
                        sw.WriteLine("height: " + item.attachments[i].photo.height);
                        sw.WriteLine("width: " + item.attachments[i].photo.width);
                        sw.WriteLine("owner_id: " + item.attachments[i].photo.owner_id);
                        sw.WriteLine("photo_1280: " + (item.attachments[i].photo.photo_1280 ?? ""));
                        sw.WriteLine("photo_2560: " + (item.attachments[i].photo.photo_2560 ?? ""));
                        sw.WriteLine("photo_130: " + (item.attachments[i].photo.photo_130 ?? ""));
                        sw.WriteLine("photo_604: " + (item.attachments[i].photo.photo_604 ?? ""));
                        sw.WriteLine("photo_75: " + (item.attachments[i].photo.photo_75 ?? ""));
                        sw.WriteLine("photo_807: " + (item.attachments[i].photo.photo_807 ?? ""));
                        sw.WriteLine("text: " + (item.attachments[i].photo.text ?? ""));
                        sw.WriteLine("user_id: " + item.attachments[i].photo.user_id);
                    }
                    else if (item.attachments[i].poll != null)
                    {
                        sw.WriteLine("id: " + item.attachments[i].poll.id);
                        sw.WriteLine("answer_id: " + item.attachments[i].poll.answer_id);
                        //sw.WriteLine("answers: " + item.attachments[i].poll.answers);
                        sw.WriteLine("created: " + item.attachments[i].poll.created);
                        sw.WriteLine("is_closed: " + item.attachments[i].poll.is_closed);
                        sw.WriteLine("owner_id: " + item.attachments[i].poll.owner_id);
                        sw.WriteLine("question: " + (item.attachments[i].poll.question ?? ""));
                        sw.WriteLine("votes: " + item.attachments[i].poll.votes);

                    }
                    else if (item.attachments[i].video != null)
                    {

                        sw.WriteLine("id: " + item.attachments[i].video.id);
                        sw.WriteLine("owner_id: " + item.attachments[i].video.owner_id);
                        sw.WriteLine("date: " + CommonHelper.GetFormattedDate(item.attachments[i].video.date));
                        sw.WriteLine("description: " + (item.attachments[i].video.description ?? ""));
                        sw.WriteLine("duration: " + item.attachments[i].video.duration);
                        sw.WriteLine("photo_130: " + (item.attachments[i].video.photo_130 ?? ""));
                        sw.WriteLine("photo_320: " + (item.attachments[i].video.photo_320 ?? ""));
                        sw.WriteLine("photo_640: " + (item.attachments[i].video.photo_640 ?? ""));
                        sw.WriteLine("player: " + (item.attachments[i].video.player ?? ""));
                        sw.WriteLine("title: " + (item.attachments[i].video.title ?? ""));
                        sw.WriteLine("views: " + item.attachments[i].video.views);
                    }
                    else if (item.attachments[i].wall != null)
                    {
                        sw.WriteLine("Wall: " + item.attachments[i].wall.id);
                        sw.WriteLine("Wall Text: " + item.attachments[i].wall.text);
                    }

                    sw.WriteLine("");
                }
            }
        }
    
        #endregion
    }
}
