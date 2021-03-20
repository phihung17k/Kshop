import React from 'react';
import { Carousel } from 'antd';
import { useState } from 'react';
import ItemCaroursel from '../ItemCaroursel/ItemCaroursel';
const Carousels = props => {
    const [imgs] = useState([
        { id:'1', src: 'https://k46.kn3.net/taringa/7/F/2/F/9/A/JUANCHISD/A51.png' },
        { id:'2', src: 'https://k46.kn3.net/taringa/7/F/2/F/9/A/JUANCHISD/A51.png' },
        { id:'3', src: 'https://k46.kn3.net/taringa/7/F/2/F/9/A/JUANCHISD/A51.png' },
        { id:'4', src: 'https://k46.kn3.net/taringa/7/F/2/F/9/A/JUANCHISD/A51.png' },
    ])
    const style={
        carousel:{
            marginBottom:'30px',
        }
    }
    return (
        <Carousel autoplay style={style.carousel}>
          {
               imgs.map( (img) => <ItemCaroursel id={img.id} src={img.src} /> )
          }
        </Carousel>
    )
}

Carousels.propTypes = {

}

export default Carousels
