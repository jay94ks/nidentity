import okhttp3.*;

import javax.net.ssl.*;
import java.net.URL;
import java.security.KeyStore;
import java.security.cert.Certificate;
import java.security.cert.CertificateFactory;

public class OkHttp_Example {

    public static OkHttpClient makeSslHttpClient() throws Exception {
        URL certPfx = Main.class.getClassLoader().getResource("cert_pfx.pfx");
        URL certCa = Main.class.getClassLoader().getResource("ca-root.crt");

        KeyStore clientStore = KeyStore.getInstance("PKCS12");
        char[] emptyPass = "".toCharArray();
        clientStore.load(certPfx.openStream(), emptyPass);

        Certificate rootCa = CertificateFactory.getInstance("X.509")
                .generateCertificate(certCa.openStream());

        KeyStore serverStore = KeyStore.getInstance("PKCS12");
        serverStore.load(null, emptyPass);
        serverStore.setCertificateEntry("root", rootCa);

        // -- build parameters.
        String defaultAlgorithm = TrustManagerFactory.getDefaultAlgorithm();
        String keyDefaultAlgorithm = KeyManagerFactory.getDefaultAlgorithm();

        TrustManagerFactory trustManagerFactory = TrustManagerFactory.getInstance(defaultAlgorithm);
        KeyManagerFactory keyManagerFactory = KeyManagerFactory.getInstance(keyDefaultAlgorithm);
        trustManagerFactory.init(clientStore);
        keyManagerFactory.init(clientStore, emptyPass);
        trustManagerFactory.init(serverStore);

        // -- build SSLContext.
        TrustManager[] trust = trustManagerFactory.getTrustManagers();
        KeyManager[] keyManagers = keyManagerFactory.getKeyManagers();

        SSLContext sslContext = SSLContext.getInstance("TLS");
        sslContext.init(keyManagers, trust, null);

        SSLSocketFactory sslSocketFactory = sslContext.getSocketFactory();
        return new OkHttpClient.Builder()
            .sslSocketFactory(sslSocketFactory, (X509TrustManager) trust[0])
            .build();
    }
}