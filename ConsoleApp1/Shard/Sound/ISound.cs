/*
*
*   This class intentionally left blank.  
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Shard
{
    public  interface ISound        
    {


        public bool play();

        public bool playNew();

        public bool pause();

        public bool unPause();

        public void stop();

        public void loop(bool b);

        public void update();

    }

}
