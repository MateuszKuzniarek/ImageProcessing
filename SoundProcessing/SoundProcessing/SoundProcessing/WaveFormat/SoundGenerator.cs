using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SoundGenerator
{
    // Header, Format, Data chunks
    WaveHeader header;
    WaveFormatChunk format;
    WaveDataChunk data;

    public SoundGenerator(List<SoundFragment> sound)
    {          
        header = new WaveHeader();
        format = new WaveFormatChunk();
        data = new WaveDataChunk();
        
        int numberOfSamples = sound.Sum(x => (int) (format.dwSamplesPerSec * format.wChannels * x.Duration));
               
        data.shortArray = new short[numberOfSamples];
    
        int amplitude = 32760;
        int counter = 0;
        foreach(SoundFragment fragment in sound)
        {
            double t = (Math.PI * 2 * fragment.Frequency) / (format.dwSamplesPerSec * format.wChannels);
            uint sampleCountForFragment = (uint)(format.dwSamplesPerSec * format.wChannels * fragment.Duration);
            for (uint i = 0; i < sampleCountForFragment - 1; i++)
            {
                for (int channel = 0; channel < format.wChannels; channel++)
                {
                    data.shortArray[counter + channel] = Convert.ToInt16(amplitude * Math.Sin(t * i));
                }

                counter++;
            }
        }
    
        data.dwChunkSize = (uint) (data.shortArray.Length* (format.wBitsPerSample / 8));
    }

    public void Save(string filePath)
    {
        // Create a file (it always overwrites)
        FileStream fileStream = new FileStream(filePath, FileMode.Create);   
     
        // Use BinaryWriter to write the bytes to the file
        BinaryWriter writer = new BinaryWriter(fileStream);
     
        // Write the header
        writer.Write(header.sGroupID.ToCharArray());
        writer.Write(header.dwFileLength);
        writer.Write(header.sRiffType.ToCharArray());
     
        // Write the format chunk
        writer.Write(format.sChunkID.ToCharArray());
        writer.Write(format.dwChunkSize);
        writer.Write(format.wFormatTag);
        writer.Write(format.wChannels);
        writer.Write(format.dwSamplesPerSec);
        writer.Write(format.dwAvgBytesPerSec);
        writer.Write(format.wBlockAlign);
        writer.Write(format.wBitsPerSample);
     
        // Write the data chunk
        writer.Write(data.sChunkID.ToCharArray());
        writer.Write(data.dwChunkSize);
        foreach (short dataPoint in data.shortArray)
        {
            writer.Write(dataPoint);
        }
     
        writer.Seek(4, SeekOrigin.Begin);
        uint filesize = (uint)writer.BaseStream.Length;
        writer.Write(filesize - 8);
        
        // Clean up
        writer.Close();
        fileStream.Close();            
    }
}