# WavAudioPlayer

Net Standard 2.0 Wav Audio Player using the winmm DLL

## Usage

``` csharp
using WavAudioPlayer;

var player = new AudioPlayer();

player.PlaySound("path_to_your_wav_file.wav");

player.PlaySound("path_to_your_wav_file.wav", 20);

player.SetVolume(50);
```
