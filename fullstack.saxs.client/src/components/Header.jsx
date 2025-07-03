import React from 'react';
import { Link } from 'react-router-dom';

export default function Header() {
    return (
        <header style={styles.header}>
            <h1 style={styles.logo}>🌐 My Systems</h1>
            <nav style={styles.nav}>
                <Link to="/" style={styles.link}>Home</Link>
                <Link to="/create" style={styles.link}>Create</Link>
                <Link to="/all" style={styles.link}>All</Link>
                <Link to="/my" style={styles.link}>Mine</Link>
            </nav>
        </header>
    );
}

const styles = {
    header: {
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        padding: '1rem 2rem',
        backgroundColor: '#282c34',
        color: 'white',
    },
    logo: {
        margin: 0,
        fontSize: '1.5rem',
    },
    nav: {
        display: 'flex',
        gap: '1rem',
    },
    link: {
        color: 'white',
        textDecoration: 'none',
        fontWeight: 'bold',
    },
};