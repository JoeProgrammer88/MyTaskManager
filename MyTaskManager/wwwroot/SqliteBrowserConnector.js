export function synchronizeFileWithIndexedDb(filename) {
    return new Promise((res) => {
        const db = window.indexedDB.open('SqliteStorage', 1);
        db.onupgradeneeded = () => {
            db.result.createObjectStore('Files', { keypath: 'id' });
        };

        db.onsuccess = () => {
            const req = db.result.transaction('Files', 'readonly').objectStore('Files').get('file');
            req.onsuccess = () => {
                Module.FS_createDataFile('/', filename, req.result, true, true, true);
                res();
            };
        };

        // DB Sync every 1000ms
        /*
        let lastModifiedTime = new Date();
        setInterval(() => {
            const path = `/${filename}`;
            if (FS.analyzePath(path).exists) {
                const mtime = FS.stat(path).mtime;
                if (mtime.valueOf() !== lastModifiedTime.valueOf()) {
                    lastModifiedTime = mtime;
                    const data = FS.readFile(path);
                    db.result.transaction('Files', 'readwrite').objectStore('Files').put(data, 'file');
                }
            }
        }, 1000);
        */
    });
}