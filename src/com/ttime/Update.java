package com.ttime;

import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.logging.Logger;
import java.util.zip.ZipEntry;
import java.util.zip.ZipInputStream;

import com.ttime.parse.Repy;

public class Update {
	static final String REPY_URI = "http://ug.technion.ac.il/rep/REPFILE.zip";
	static final int BLOCKSIZE = 1024;

	static void downloadRepy() throws IOException {
		Logger log = Logger.getLogger("global");
		log.info(String.format("Downloading REPY file from %s", REPY_URI));
		URL u;

		try {
			u = new URL(REPY_URI);
		} catch (MalformedURLException e) {
			e.printStackTrace();
			throw new RuntimeException(e);
		}

		InputStream is = u.openStream();
		FileOutputStream f = new FileOutputStream(Repy.DEFAULT_PATH);

		byte data[] = new byte[BLOCKSIZE];

		ZipInputStream zi = new ZipInputStream(is);

		ZipEntry ze = zi.getNextEntry();

		while (ze != null && !ze.getName().equals("REPY")) {
			ze = zi.getNextEntry();
		}

		if (ze == null) {
			throw new IOException("REPY file missing from REPFILE.zip");
		}

		int count;

		while ((count = zi.read(data, 0, BLOCKSIZE)) != -1) {
			f.write(data, 0, count);
		}

		f.close();
	}
}
