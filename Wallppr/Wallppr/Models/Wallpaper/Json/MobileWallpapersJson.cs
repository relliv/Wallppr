using System;
using System.Collections.Generic;

namespace Wallppr.Models.Wallpaper.Json
{
    public class Resolution
    {
        public int height { get; set; }
        public int width { get; set; }
    }

    public class Adapted
    {
        public Resolution resolution { get; set; }
        public int size { get; set; }
        public string url { get; set; }
    }

    public class AdaptedLandscape
    {
        public Resolution resolution { get; set; }
        public int size { get; set; }
        public string url { get; set; }
    }

    public class Original
    {
        public Resolution resolution { get; set; }
        public int size { get; set; }
        public string url { get; set; }
    }

    public class PreviewSmall
    {
        public Resolution resolution { get; set; }
        public int size { get; set; }
        public string url { get; set; }
    }

    public class Variations
    {
        public Adapted adapted { get; set; }
        public AdaptedLandscape adapted_landscape { get; set; }
        public Original original { get; set; }
        public PreviewSmall preview_small { get; set; }
    }

    public class Item
    {
        public string author { get; set; }
        public int category_id { get; set; }
        public string content_type { get; set; }
        public string description { get; set; }
        public int downloads { get; set; }
        public int id { get; set; }
        public string license { get; set; }
        public int rating { get; set; }
        public string source_link { get; set; }
        public IList<string> tags { get; set; }
        public DateTime uploaded_at { get; set; }
        public Variations variations { get; set; }
    }

    public class MobileWallpapersJson
    {
        public int count { get; set; }
        public IList<Item> items { get; set; }
        public DateTime response_time { get; set; }
    }
}