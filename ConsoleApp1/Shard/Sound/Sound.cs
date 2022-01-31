/*
*
*   This class intentionally left blank.  
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Shard
{
    public abstract class Sound        
    {

        private protected bool playing = false;
        private protected bool paused = false;
        private protected bool looping = false;

        public bool Looping { get => looping;}
        private bool Paused { get => paused;}

        private bool Playing { get => playing; }

        public abstract bool play();

        public abstract bool playNew();

        public abstract bool pause();

        public abstract bool unPause();

        public abstract void stop();

        public abstract void loop(bool b);

    }

}
