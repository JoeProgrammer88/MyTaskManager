export function synchronizeFileWithIndexedDb(filename) {
    return new Promise((res, rej) => {
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

        /*
         * Autosync database
        // DB Sync every 30 seconds
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
        }, 30000);
        */
    });
}

export function saveDatabaseToBrowser(filename) {
    return new Promise((res, rej) => {
        const path = `/${filename}`;
        if (FS.analyzePath(path).exists) {
            const data = FS.readFile(path);
            const db = window.indexedDB.open('SqliteStorage', 1);
            db.onsuccess = () => {
                db.result.transaction('Files', 'readwrite').objectStore('Files').put(data, 'file');
                res();
            }  
        }
    });
}