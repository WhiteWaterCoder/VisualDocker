using System;
using VisualDocker.Infrastructure;

namespace VisualDocker.Models
{
    public class DockerImageModel : NotifyPropertyChangedObject
    {
        private string _imageId;
        private string _repository;
        private string _tag;
        private string _digest;
        private string _createdSince;
        private DateTime _createdAt;
        private string _size;

        public string ImageId
        {
            get { return _imageId; }
            private set { Set(ref _imageId, value); }
        }

        public string Repository
        {
            get { return _repository; }
            private set { Set(ref _repository, value); }
        }

        public string Tag
        {
            get { return _tag; }
            private set { Set(ref _tag, value); }
        }

        public string Digest
        {
            get { return _digest; }
            private set { Set(ref _digest, value); }
        }

        public string CreatedSince
        {
            get { return _createdSince; }
            private set { Set(ref _createdSince, value); }
        }

        public DateTime CreatedAt
        {
            get { return _createdAt; }
            private set { Set(ref _createdAt, value); }
        }

        public string Size
        {
            get { return _size; }
            private set { Set(ref _size, value); }
        }

        public DockerImageModel(string imageId, string repository, string tag, string digest, string createdSince, DateTime createdAt, string size)
        {
            ImageId = imageId;
            Repository = repository;
            Tag = tag;
            Digest = digest;
            CreatedSince = createdSince;
            CreatedAt = createdAt;
            Size = size;
        }

        public override string ToString()
        {
            return Repository;
        }
    }
}