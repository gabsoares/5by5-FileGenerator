using Repositories;
using Services;

namespace Controllers
{
    public class RadarController
    {
        private RadarService _radarService;

        public RadarController()
        {
            _radarService = new RadarService();
        }
    }
}
