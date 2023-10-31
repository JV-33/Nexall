import { NavLink, Outlet } from "react-router-dom"

export default function RootLayout () {
    return(
         <div className="root-layout">
            <header>
                <nav>
                    <h1>Nexall</h1>
                    <NavLink to="/" activeClassName="active">Home</NavLink>
                    <NavLink to = "saraksts">Saraksts</NavLink>
                    <NavLink to = "filter">Filter</NavLink>
                    <NavLink to = "daystats/:date?">Day Stats</NavLink>
                </nav>
            </header>
            <main>
                <Outlet />
            </main>
        </div>
    )
}