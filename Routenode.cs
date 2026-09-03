namespace WayPoints
{
    class RouteNode
    {
        private WayPoint data;
        private RouteNode? next;   

        public RouteNode(WayPoint wp)
        {
            this.data = wp;
            this.next = null;      
        }

        public RouteNode(WayPoint wp, RouteNode? next) 
        {
            this.data = wp;
            this.next = next;
        }

        public WayPoint Data
        {
            get { return data; }
            set { data = value; }
        }

        public RouteNode? Next     
        {
            get { return next; }
            set { next = value; }
        }
    }
}
