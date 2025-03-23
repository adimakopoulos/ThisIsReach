

namespace ThisIsReach
{
    public interface IBuilding 
    {
        // Property declaration:
        int Name
        {
            get;
            set;
        }
        public void BuildStructure();
        public void SelecteBuilding();
        public void DeselecteBuilding();
        public void CreateWorker();

    }
}
