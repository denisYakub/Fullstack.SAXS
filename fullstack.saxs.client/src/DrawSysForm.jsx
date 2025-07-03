import { useState } from "react";
import SysView from "./SysView";

export default function DrawSysForm() {
    const [systemId, setSystemId] = useState("");
    const [inputId, setInputId] = useState("");
    const [data, setData] = useState(null);

    const handleLoadSystem = () => {
        if (!inputId) return;
        fetch(`/api/saxs/sys/${inputId}`)
            .then((res) => res.json())
            .then((data) => {
                setSystemId(inputId);
                setData(data);
            })
            .catch(console.error);
    };

    return (
        <div>
            <div style={{ margin: '1rem' }}>
                <input
                    type="text"
                    placeholder="Put SYSTEM_ID"
                    value={inputId}
                    onChange={(e) => setInputId(e.target.value)}
                    style={{ padding: '0.5rem', marginRight: '0.5rem' }}
                />
                <button onClick={handleLoadSystem}>Load</button>
            </div>

            {data && (
                <div style={{ height: '100vh', width: '100vh' }}>
                    <SysView data={data} />
                </div>
            )}
        </div>
    );
};
