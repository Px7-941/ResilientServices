﻿using System;
using System.Collections.Generic;

namespace ResilientServices.Dtos
{
    public class PhotoDto
    {
        public int AlbumId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}