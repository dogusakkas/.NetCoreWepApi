using Entities.LinkModels;
using Entities.Models;

namespace Entities.LogModel
{
    public class LinkResponse
    {
        public bool HasLinks { get; set; }
        public List<Entity> ShapedEntities { get; set; }
        public LinkCollectionWrapper<Entity> LinkedEntites { get; set; }
        public LinkResponse()
        {
            ShapedEntities = new List<Entity>();
            LinkedEntites = new LinkCollectionWrapper<Entity>();
        }
    }
}
