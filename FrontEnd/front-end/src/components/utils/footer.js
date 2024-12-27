import React from 'react';

class Footer extends React.Component {

    constructor(props) {
        super(props)
    }

    render() {
        return (
            <div className="container-fluid pt-4 px-4">
            <div className="bg-light rounded-top p-4">
              <div className="row">
                <div className="col-12 col-sm-6 text-center text-sm-start">
                  © 8180378-Hugo Silva
                </div>
                
              </div>
            </div>
          </div>
        )
    }
}


export default Footer

