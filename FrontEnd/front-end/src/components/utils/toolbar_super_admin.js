import React from 'react'
import './navbar.css'
class Toolbar extends React.Component {

    constructor(props) {
        super(props)
        this.state = {
            imagesrc: "",
            photofilename: "anonymous.png",
            showMenu: false
        }

        this.state.photofilename = "anonymous.png";
        this.state.imagesrc = localStorage.getItem("photopath");
        if (this.state.imagesrc == undefined) {
            this.state.imagesrc = process.env.REACT_APP_PHOTOPATH + this.state.photofilename;
        }
    }
    change = (e) => {
        this.setState({ showMenu: !this.state.showMenu })

    }

    //Logout App
    logOut = (e) => {
        localStorage.setItem("token", "");
        localStorage.setItem("id",  "");
        localStorage.setItem("photopath",  "");
        localStorage.setItem("name",  "")
        window.location.href = '/';
    }

    render() {
        return (
            <>
                <nav className="navbar navbar-expand bg-light navbar-light sticky-top px-4 py-0">
                    {this.state.showMenu == true &&
                        <>
                            <div className="menu">
                            <a href="/home" className="nav-item nav-link active" style={{ fontSize: '20px' }}><i className="fa fa-home me-2" />Home</a>
                                <a href="/adduser" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fas fa-edit">  Adicionar Utilizadores</i> </a>
                                <a href="/listemployee" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fa fa-address-card"  >  Visualizar Funcionários </i></a>
                                <a href="/listcustomer" className="dropdown-item" style={{ fontSize: '18px' }}><i class="fa fa-credit-card">  Visualizar Clientes </i></a>
                                <a href="/addorganization" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fas fa-edit">  Adicionar Organização</i> </a>
                                <a href="/listorganization" className="dropdown-item" style={{ fontSize: '18px' }}> <i class="fa fa-address-card">  Visualizar Organizações </i></a>
                            </div>

                        </>
                    }
                    <a href="/home" className="navbar-brand d-flex d-lg-none me-4">
                        <h2 className="text-primary mb-0"><i className="fa fa-hashtag" /></h2>
                    </a>

                    <a onClick={this.change} className="sidebar-toggler flex-shrink-0">
                        <i className="fa fa-bars" />
                    </a>

                    <div className="navbar-nav align-items-center ms-auto">
                        <div className="nav-item dropdown">
                            <a href="#" className="nav-link dropdown-toggle" data-bs-toggle="dropdown">
                                <img className="rounded-circle me-lg-2" src={this.state.imagesrc} alt="" style={{ width: '40px', height: '40px' }} />
                                <span className="d-none d-lg-inline-flex">{localStorage.getItem("name")}</span>
                            </a>
                            <div className="dropdown-menu dropdown-menu-end bg-light border-0 rounded-0 rounded-bottom m-0">
                            <a onClick={this.logOut} className="dropdown-item">Log Out</a>
                            </div>
                        </div>
                    </div>
                </nav>
            </>
        )
    }
}


export default Toolbar

