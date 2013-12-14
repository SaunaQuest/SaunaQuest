#pragma strict



function Update () {
if ( Input.GetButtonDown( "Horizontal" ) || Input.GetButtonDown( "Vertical" ) )
        audio.Play();
    else if ( !Input.GetButton( "Horizontal" ) && !Input.GetButton( "Vertical" ) && audio.isPlaying )
        audio.Pause(); // or Pause()

}