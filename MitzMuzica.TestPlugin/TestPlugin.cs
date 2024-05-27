/**************************************************************************
 *                                                                        *
 *  Description: A test plugin that offers no real functionality          *
 *  Website:     https://github.com/RealKC/mitzmuzica                     *
 *  Copyright:   (c) 2024, Mițca Dumitru                                  *
 *  SPDX-License-Identifier: AGPL-3.0-only                                *
 *                                                                        *
 **************************************************************************/

using MitzMuzica.PluginAPI;

namespace MitzMuzica.TestPlugin;

public class TestPlugin : IAudioPlugin
{
    public string Name => "TestPlugin";

    public IAudioFile Open(string path)
    {
        return new TestAudioFile(path);
    }

    private class TestAudioFile : IAudioFile
    {
        internal TestAudioFile(string path)
        {
            Title = path;
            Author = "kc";
            Length = 420;
        }

        public string Title { get; }

        public string Author { get; }
        public long Length { get; }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void SeekTo(long s)
        {
            throw new NotImplementedException();
        }
    }
}
